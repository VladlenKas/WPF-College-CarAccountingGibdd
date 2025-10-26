using System;
using System.IO;
using System.Text;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using CarAccountingGibdd.Model;

namespace CarAccountingGibdd.Classes.Services
{
    public class DocumentService
    {

        // Свидетельство
        public static void GenerateCertificateReport(string outputPath, Certificate certificate, string employeeFullname)
        {
            // Подключаем кодировки, если нужно
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var writer = new PdfWriter(outputPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4);
            document.SetMargins(57, 42, 57, 85);

            // Подключаем шрифты
            var baseFont = PdfFontFactory.CreateFont("c:\\windows\\fonts\\times.ttf", "Identity-H");
            var boldFont = PdfFontFactory.CreateFont("c:\\windows\\fonts\\timesbd.ttf", "Identity-H");

            var russianFont = baseFont;
            var headerFont = boldFont;
            var subHeaderFont = boldFont;
            var normalFont = baseFont;
            var boldNormalFont = boldFont;

            // Логотип
            Image logo = null;
            try
            {
                var uri = new Uri("/CarAccountingGibdd;component/Resources/LogoGibdd.png", UriKind.Relative);
                var streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        logo = new Image(ImageDataFactory.Create(imageBytes));
                        logo.ScaleToFit(55f, 55f);
                        logo.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                    }
                }
            }
            catch { /* Если нет изображения — ничего страшного */ }

            // Таблица заголовка
            Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4 })).UseAllAvailableWidth();
            headerTable.SetWidth(260);
            headerTable.SetHorizontalAlignment(HorizontalAlignment.LEFT);

            // Левая ячейка — логотип
            Cell logoCell = new Cell().SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
            if (logo != null) logoCell.Add(logo);
            headerTable.AddCell(logoCell);

            // Правая ячейка — название организации
            var orgName = new Paragraph("Департамент Государственной инспекции безопасности дорожного движения\n(ГИБДД)")
                .SetFont(boldNormalFont)
                .SetFontSize(12)
                .SetMultipliedLeading(1f)
                .SetMargin(0)
                .SetPadding(0);

            Cell orgNameCell = new Cell().Add(orgName)
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetPaddingBottom(7)
                .SetPaddingLeft(15);
            headerTable.AddCell(orgNameCell);

            document.Add(headerTable);

            // Заголовок
            document.Add(new Paragraph("Свидетельство о регистрации транспортного средства")
                .SetFont(headerFont).SetFontSize(16)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(36)
                .SetMarginBottom(12));

            // Номер свидетельства и дата выдачи
            var certInfo = new Paragraph($"№ {certificate.Number}\nДата выдачи: {certificate.IssueDate:dd.MM.yyyy}")
                .SetFont(subHeaderFont).SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20);
            document.Add(certInfo);

            // Владелец ТС
            var owner = certificate.Application.Owner;
            document.Add(new Paragraph("Владелец ТС:").SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(5));
            document.Add(new Paragraph(
                $"ФИО: {owner.Fullname}\n" +
                $"Паспорт: {owner.Passport}\n" +
                $"Адрес: {owner.Address}\n" +
                $"Телефон: {owner.Phone}\n" +
                $"Эл. почта: {owner.EmailValue}")
                .SetFont(normalFont).SetFontSize(12).SetMarginBottom(15));

            // Транспортное средство
            var vehicle = certificate.Application.Vehicle;
            document.Add(new Paragraph("Транспортное средство:").SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(5));
            document.Add(new Paragraph(
                $"Марка, модель: {vehicle.Brand} {vehicle.Model} {vehicle.Year}\n" +
                $"Цвет: {vehicle.Color}\n" +
                $"VIN: {vehicle.Vin}\n" +
                $"Гос. номер: {(certificate.LicensePlate)}\n" +
                $"Тип ТС: {vehicle.VehicleType.Name}\n" +
                $"Поддержанное: {vehicle.UsedValueString}")
                .SetFont(normalFont).SetFontSize(12).SetMarginBottom(15));

            // Информация о заявлении
            var app = certificate.Application;
            document.Add(new Paragraph("Информация о заявлении:").SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(5));
            document.Add(new Paragraph(
                $"Номер заявки: {app.ApplicationId}\n" +
                $"Дата подачи: {app.DatetimeSupply:dd.MM.yyyy}\n" +
                $"Инспектор: {app.InspectorFullname}\n" +
                $"Департамент: {app.DepartmentName}")
                .SetFont(normalFont).SetFontSize(12).SetMarginBottom(15));

            // Статус свидетельства
            document.Add(new Paragraph($"Статус свидетельства: {certificate.IsActiveName}")
                .SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(20));

            // Дата составления и сотрудник
            document.Add(new Paragraph(
                $"Дата составления отчета: {DateTime.Now:dd.MM.yyyy}\n" +
                $"Ответственный сотрудник: {employeeFullname}")
                .SetFont(normalFont).SetFontSize(12)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginBottom(20));

            // Линия для подписи и надпись
            Table signatureLineTable = new Table(1).SetWidth(200).SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            Cell lineCell = new Cell()
                .SetBorderTop(new SolidBorder(1))
                .SetBorderBottom(Border.NO_BORDER)
                .SetBorderLeft(Border.NO_BORDER)
                .SetBorderRight(Border.NO_BORDER)
                .SetHeight(5)  // уменьшенная высота
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM)
                .SetPadding(0)
                .SetMargin(0);
            signatureLineTable.AddCell(lineCell);

            var signatureText = new Paragraph("(подпись)")
                .SetFont(russianFont)
                .SetFontSize(10)
                .SimulateItalic()
                .SetMargin(0)
                .SetMultipliedLeading(1);

            Cell captionCell = new Cell()
                .Add(signatureText)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(0)
                .SetMargin(0);
            signatureLineTable.AddCell(captionCell);

            document.Add(signatureLineTable);

            document.Close();
        }

        // Список нарушений
        public static void GenerateViolationsReport(string outputPath, int inspectionId, string employeeFullname)
        {
            var inspection = App.DbContext.Inspections.Single(r => r.InspectionId == inspectionId);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var writer = new PdfWriter(outputPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4);
            document.SetMargins(57, 42, 57, 85);

            // Шрифты
            var baseFont = PdfFontFactory.CreateFont("c:\\windows\\fonts\\times.ttf", "Identity-H");
            var boldFont = PdfFontFactory.CreateFont("c:\\windows\\fonts\\timesbd.ttf", "Identity-H");

            var russianFont = baseFont;
            var headerFont = boldFont;
            var subHeaderFont = boldFont;
            var normalFont = baseFont;
            var boldNormalFont = boldFont;

            // Логотип
            Image logo = null;
            try
            {
                var uri = new Uri("/CarAccountingGibdd;component/Resources/LogoGibdd.png", UriKind.Relative);
                var streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        logo = new Image(ImageDataFactory.Create(imageBytes));
                        logo.ScaleToFit(55f, 55f);
                        logo.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                    }
                }
            }
            catch { /* Если нет изображения — ничего страшного */ }

            // Таблица заголовка
            Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4 })).UseAllAvailableWidth();
            headerTable.SetWidth(260);
            headerTable.SetHorizontalAlignment(HorizontalAlignment.LEFT);

            // Левая ячейка — логотип
            Cell logoCell = new Cell().SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
            if (logo != null) logoCell.Add(logo);
            headerTable.AddCell(logoCell);

            // Правая ячейка — название организации
            var orgName = new Paragraph("Департамент Государственной инспекции безопасности дорожного движения\n(ГИБДД)")
                .SetFont(boldNormalFont)
                .SetFontSize(12)
                .SetMultipliedLeading(1f)
                .SetMargin(0)
                .SetPadding(0);

            Cell orgNameCell = new Cell().Add(orgName)
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetPaddingBottom(7)
                .SetPaddingLeft(15);
            headerTable.AddCell(orgNameCell);

            document.Add(headerTable);

            // Заголовок
            document.Add(new Paragraph("Отчёт о выявленных нарушениях")
                .SetFont(headerFont).SetFontSize(16)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(36)
                .SetMarginBottom(12));

            // Номер отчёта и дата выдачи
            var certInfo = new Paragraph($"№ {inspection.InspectorId}\nДата выдачи: {inspection.DatetimeCompleted:dd.MM.yyyy}")
                .SetFont(subHeaderFont).SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20);
            document.Add(certInfo);

            // Информация о владельце и ТС
            var owner = inspection.Application.Owner;
            var vehicle = inspection.Application.Vehicle;

            document.Add(new Paragraph("Информация о владельце и ТС:").SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(5));

            document.Add(new Paragraph(
                $"ФИО: {owner.Fullname}\n" +
                $"Телефон: {owner.Phone}\n" +
                $"Эл. почта: {owner.EmailValue}\n" +
                $"ТС: {vehicle.Info}\n" +
                $"VIN: {vehicle.Vin}")
                .SetFont(normalFont).SetFontSize(12).SetMarginBottom(15));

            // Информация об инспекции
            document.Add(new Paragraph("Информация об инспекции:").SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(5));

            document.Add(new Paragraph(
                $"Номер заявки: {inspection.ApplicationId}\n" +
                $"Номер инспекции: {inspection.InspectionId}\n" +
                $"Дата и время: {inspection.DatetimePlanned:dd.MM.yyyy HH:mm}\n" +
                $"Инспектор: {inspection.Inspector.Fullname}\n" +
                $"Департамент: {inspection.Application.DepartmentName}")
                .SetFont(normalFont).SetFontSize(12).SetMarginBottom(15));

            // Выявленные нарушения
            document.Add(new Paragraph("Выявленные нарушения:").SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(5));

            // Нумерованный список нарушений
            var violationsList = new List(ListNumberingType.DECIMAL);
            foreach (var violationInspection in inspection.ViolationsInspections)
            {
                var violation = violationInspection.Violation;
                violationsList.Add((ListItem)new ListItem(violation.NumberDescription).SetFont(normalFont).SetFontSize(12));
            }
            document.Add(violationsList);

            // Количество нарушений
            document.Add(new Paragraph($"Всего нарушений: {inspection.ViolationsInspections.Count()}")
                .SetFont(boldNormalFont).SetFontSize(12)
                .SetMarginTop(15)
                .SetMarginBottom(20));

            // Дата составления и ответственный сотрудник
            document.Add(new Paragraph(
                $"Дата составления отчета: {DateTime.Now:dd.MM.yyyy}\n" +
                $"Ответственный сотрудник: {employeeFullname}")
                .SetFont(normalFont).SetFontSize(12)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginBottom(20));

            // Таблица с линией для подписи и подписью
            Table signatureLineTable = new Table(1).SetWidth(200).SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            Cell lineCell = new Cell()
                .SetBorderTop(new SolidBorder(1))
                .SetBorderBottom(Border.NO_BORDER)
                .SetBorderLeft(Border.NO_BORDER)
                .SetBorderRight(Border.NO_BORDER)
                .SetHeight(5)  // уменьшенная высота линии
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM)
                .SetPadding(0)
                .SetMargin(0);
            signatureLineTable.AddCell(lineCell);

            var signatureText = new Paragraph("(подпись)")
                .SetFont(russianFont)
                .SetFontSize(10)
                .SimulateItalic()
                .SetMargin(0)
                .SetMultipliedLeading(1);

            Cell captionCell = new Cell()
                .Add(signatureText)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(0)
                .SetMargin(0);
            signatureLineTable.AddCell(captionCell);

            document.Add(signatureLineTable);

            document.Close();
        }

        // Отчет PDF
        public static void GeneratePdfReport(string outputPath, List<Report> reports, DateOnly startDate, DateOnly endDate, string employeeFullname)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var writer = new PdfWriter(outputPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4);
            document.SetMargins(57, 42, 57, 85);

            // Шрифты
            var baseFont = PdfFontFactory.CreateFont("c:\\windows\\fonts\\times.ttf", "Identity-H");
            var boldFont = PdfFontFactory.CreateFont("c:\\windows\\fonts\\timesbd.ttf", "Identity-H");

            // Стили
            var russianFont = baseFont;
            var headerFont = boldFont;
            var subHeaderFont = boldFont;
            var normalFont = baseFont;
            var boldNormalFont = boldFont;
            var boldHeaderTableFont = boldFont;
            var cellFont = baseFont;

            // Логотип
            Image logo = null;
            try
            {
                var uri = new Uri("/CarAccountingGibdd;component/Resources/LogoGibdd.png", UriKind.Relative);
                var streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        logo = new Image(ImageDataFactory.Create(imageBytes));
                        logo.ScaleToFit(55f, 55f);
                        logo.SetHorizontalAlignment(HorizontalAlignment.LEFT);
                    }
                }
            }
            catch { /* Если нет изображения — ничего страшного */ }

            // Шапка с логотипом и названием
            Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 4 })).UseAllAvailableWidth();
            headerTable.SetWidth(260);
            headerTable.SetHorizontalAlignment(HorizontalAlignment.LEFT);

            Cell logoCell = new Cell().SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
            if (logo != null) logoCell.Add(logo);
            headerTable.AddCell(logoCell);

            // Заголовок с уменьшенным межстрочным интервалом
            var orgName = new Paragraph("Департамент Государственной инспекции безопасности дорожного движения\n(ГИБДД)")
                .SetFont(boldNormalFont)
                .SetFontSize(12)
                .SetMultipliedLeading(1f)
                .SetMargin(0)
                .SetPadding(0);

            Cell orgNameCell = new Cell().Add(orgName)
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetPaddingBottom(7)
                .SetPaddingLeft(15);
            headerTable.AddCell(orgNameCell);

            document.Add(headerTable);

            // Название документа
            document.Add(new Paragraph($"Отчет по заявлениям за период с {startDate} по {endDate}")
                .SetFont(headerFont).SetFontSize(16)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(36)
                .SetMarginBottom(12));

            // Таблица с отчетными данными
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1f, 1.5f, 1.7f, 2f, 1.7f, 1.7f, 1.5f })).UseAllAvailableWidth();

            // Заголовки столбцов
            string[] headers = { "№ заявки", "Департамент", "Владелец", "Данные ТС", "Дата подачи", "Дата рассмотрения", "Статус" };
            foreach (var h in headers)
            {
                var cell = new Cell()
                    .Add(new Paragraph(h).SetFont(boldHeaderTableFont).SetFontSize(10))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetBackgroundColor(new DeviceRgb(230, 230, 230))
                    .SetPadding(4);
                table.AddCell(cell);
            }

            // Данные
            foreach (var r in reports)
            {
                table.AddCell(new Cell().Add(new Paragraph(r.ApplcationId.ToString()).SetFont(cellFont).SetFontSize(10)));
                table.AddCell(new Cell().Add(new Paragraph(r.DepartmentName).SetFont(cellFont).SetFontSize(10)));
                table.AddCell(new Cell().Add(new Paragraph(r.OwnerFullname).SetFont(cellFont).SetFontSize(10)));
                table.AddCell(new Cell().Add(new Paragraph(r.VehicleFullInfo).SetFont(cellFont).SetFontSize(10)));
                table.AddCell(new Cell().Add(new Paragraph(r.DatetimeSupply.ToString("dd.MM.yyyy HH:mm")).SetFont(cellFont).SetFontSize(10)));
                table.AddCell(new Cell().Add(new Paragraph(r.DatetimeConfirmValue).SetFont(cellFont).SetFontSize(10)));
                table.AddCell(new Cell().Add(new Paragraph(r.StatusName).SetFont(cellFont).SetFontSize(10)));
            }
            document.Add(table);

            // Данные по заявкам
            document.Add(new Paragraph("Данные о заявлениях:").SetFont(boldNormalFont).SetFontSize(12).SetMarginTop(15).SetMarginBottom(5));
            document.Add(new Paragraph(
                $"В работе: {reports.Count(r => r.StatusName != "Отклонена" && r.StatusName != "Требует доработки" && r.StatusName != "Завершена")} заявлений\n" +
                $"Отклонено: {reports.Count(r => r.StatusName == "Отклонена")} заявлений\n" +
                $"Имеют нарушения: {reports.Count(r => r.StatusName == "Требует доработки")} заявлений\n" +
                $"Выдано свидетельств: {reports.Count(r => r.StatusName == "Завершена")} шт.\n"
                ).SetFont(normalFont).SetFontSize(12).SetMarginBottom(20));

            // Всего заявок
            document.Add(new Paragraph($"Всего поступило: {reports.Count()} заявлений")
                .SetFont(boldNormalFont).SetFontSize(12).SetMarginBottom(20));

            // Дата составления и ответственный
            document.Add(new Paragraph(
                $"Дата составления отчета: {DateTime.Now:dd.MM.yyyy}\n" +
                $"Ответственный сотрудник: {employeeFullname}")
                .SetFont(normalFont).SetFontSize(12)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginBottom(20));

            // Линия для подписи и надпись
            Table signatureLineTable = new Table(1).SetWidth(200).SetHorizontalAlignment(HorizontalAlignment.RIGHT);

            Cell lineCell = new Cell()
                .SetBorderTop(new SolidBorder(1))
                .SetBorderBottom(Border.NO_BORDER)
                .SetBorderLeft(Border.NO_BORDER)
                .SetBorderRight(Border.NO_BORDER)
                .SetHeight(5)  // уменьшенная высота
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM)
                .SetPadding(0)
                .SetMargin(0);
            signatureLineTable.AddCell(lineCell);

            var signatureText = new Paragraph("(подпись)")
                .SetFont(russianFont)
                .SetFontSize(10)
                .SimulateItalic()
                .SetMargin(0)
                .SetMultipliedLeading(1);

            Cell captionCell = new Cell()
                .Add(signatureText)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(0)
                .SetMargin(0);
            signatureLineTable.AddCell(captionCell);

            document.Add(signatureLineTable);

            document.Close();
        }
    }
}
