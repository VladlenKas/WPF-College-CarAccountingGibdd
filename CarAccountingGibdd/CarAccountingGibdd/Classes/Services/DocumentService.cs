using CarAccountingGibdd.Model;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System;
using System.Windows;

namespace CarAccountingGibdd.Classes.Services
{
    public class DocumentService
    {

        // Свидетельство
        public static void GenerateCertificateReport(string outputPath, Certificate certificate, string employeeFullname)
        {
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var document = new Document(PageSize.A4, 85f, 42f, 57f, 57f))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont boldFont = BaseFont.CreateFont("c:\\windows\\fonts\\timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                Font russianFont = new Font(baseFont, 10f, Font.ITALIC);
                Font headerFont = new Font(boldFont, 16f, Font.NORMAL);
                Font subHeaderFont = new Font(boldFont, 14f, Font.NORMAL);
                Font normalFont = new Font(baseFont, 12f, Font.NORMAL);
                Font boldNormalFont = new Font(boldFont, 12f, Font.NORMAL);

                Image logo = null;
                var uri = new Uri("/CarAccountingGibdd;component/Resources/LogoGibdd.png", UriKind.Relative);
                var streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        logo = Image.GetInstance(imageBytes);
                        logo.ScaleToFit(55f, 55f);
                        logo.Alignment = Image.ALIGN_LEFT;
                    }
                }

                // Создаем таблицу с 2 колонками для логотипа и названия
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.TotalWidth = 260f;
                headerTable.LockedWidth = true;
                headerTable.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTable.SetWidths(new float[] { 1f, 4f }); // ширина колонок (логотип меньше)

                // Левая ячейка - логотип
                PdfPCell logoCell = new PdfPCell();
                if (logo != null)
                {
                    logoCell.AddElement(logo);
                }
                logoCell.Border = PdfPCell.NO_BORDER;
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerTable.AddCell(logoCell);

                // Правая ячейка - название организации
                PdfPCell orgNameCell = new PdfPCell(new Phrase(14f, "Департамент Государственной инспекции безопасности дорожного движения\n(ГИБДД)", boldNormalFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingBottom = 7f,
                    PaddingLeft = 15f
                };
                headerTable.AddCell(orgNameCell);

                // Добавляем таблицу в документ
                document.Add(headerTable);

                // Заголовок
                Paragraph title = new Paragraph("Свидетельство о регистрации транспортного средства", headerFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 20f,
                    SpacingAfter = 15f
                };
                document.Add(title);

                // Номер свидетельства и дата выдачи
                Paragraph certInfo = new Paragraph(
                    $"№ {certificate.Number}\nДата выдачи: {certificate.IssueDate:dd.MM.yyyy}",
                    subHeaderFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(certInfo);

                // Владелец ТС
                var owner = certificate.Application.Owner;
                Paragraph ownerHeader = new Paragraph("Владелец ТС:", boldNormalFont) { SpacingAfter = 5f };
                document.Add(ownerHeader);

                Paragraph ownerInfo = new Paragraph(
                    $"ФИО: {owner.Fullname}\n" +
                    $"Паспорт: {owner.Passport}\n" +
                    $"Адрес: {owner.Address}\n" +
                    $"Телефон: {owner.Phone}\n" +
                    $"Эл. почта: {owner.EmailValue}",
                    normalFont)
                {
                    SpacingAfter = 15f
                };
                document.Add(ownerInfo);

                // Транспортное средство
                var vehicle = certificate.Application.Vehicle;
                Paragraph vehicleHeader = new Paragraph("Транспортное средство:", boldNormalFont) { SpacingAfter = 5f };
                document.Add(vehicleHeader);

                Paragraph vehicleInfo = new Paragraph(
                    $"Марка, модель: {vehicle.Brand} {vehicle.Model} {vehicle.Year}\n" +
                    $"Цвет: {vehicle.Color}\n" +
                    $"VIN: {vehicle.Vin}\n" +
                    $"Гос. номер: {(certificate.LicensePlate)}\n" +
                    $"Тип ТС: {vehicle.VehicleType.Name}\n" +
                    $"Поддержанное: {vehicle.UsedValueString}",
                    normalFont)
                {
                    SpacingAfter = 15f
                };
                document.Add(vehicleInfo);

                // Информация о заявлении
                var app = certificate.Application;
                Paragraph appHeader = new Paragraph("Информация о заявлении:", boldNormalFont) { SpacingAfter = 5f };
                document.Add(appHeader);

                Paragraph appInfo = new Paragraph(
                    $"Номер заявки: {app.ApplicationId}\n" +
                    $"Дата подачи: {app.DatetimeSupply:dd.MM.yyyy}\n" +
                    $"Инспектор: {app.InspectorFullname}\n" +
                    $"Департамент: {app.DepartmentName}",
                    normalFont)
                {
                    SpacingAfter = 15f
                };
                document.Add(appInfo);

                // Статус свидетельства
                Paragraph status = new Paragraph($"Статус свидетельства: {certificate.IsActiveName}", boldNormalFont)
                {
                    SpacingAfter = 20f
                };
                document.Add(status);

                // Дата составления и администратор
                Paragraph footer = new Paragraph(
                    $"Дата составления отчета: {DateTime.Now:dd.MM.yyyy}\n" +
                    $"Ответственный сотрудник: {employeeFullname}",
                    normalFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                };
                document.Add(footer);

                // Добавляем таблицу с линией и надписью (как раньше)
                PdfPTable signatureLineTable = new PdfPTable(1);
                signatureLineTable.TotalWidth = 200f;
                signatureLineTable.LockedWidth = true;
                signatureLineTable.HorizontalAlignment = Element.ALIGN_RIGHT;

                PdfPCell lineCell = new PdfPCell()
                {
                    BorderWidthBottom = 1f,
                    Border = PdfPCell.BOTTOM_BORDER,
                    FixedHeight = 20f,
                    Padding = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_BOTTOM
                };
                signatureLineTable.AddCell(lineCell);

                PdfPCell captionCell = new PdfPCell(new Phrase("(подпись)", russianFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 2f
                };
                signatureLineTable.AddCell(captionCell);

                document.Add(signatureLineTable);

                // Получаем текущую вертикальную позицию (нижний край таблицы)
                float yPos = writer.GetVerticalPosition(true);

                // Вычисляем координаты для изображения подписи
                float xPos = document.PageSize.Width - document.RightMargin - 200f + 40f; // подгоните под ширину таблицы и отступы
                float yImage = yPos + 5f; // немного выше линии, чтобы наложиться

                /*// Загружаем изображение подписи
                Image signatureImage = null;
                uri = new Uri("/CarAccountingGibdd;component/Resources/Signature.png", UriKind.Relative);
                streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        signatureImage = Image.GetInstance(imageBytes);
                        signatureImage.ScaleToFit(114f, 114f);
                    }
                }

                if (signatureImage != null)
                {
                    signatureImage.SetAbsolutePosition(xPos, yImage);
                    PdfContentByte cb = writer.DirectContent;
                    PdfGState graphicsState = new PdfGState();
                    graphicsState.FillOpacity = 0.7f;  // 70% прозрачность
                    cb.SetGState(graphicsState);
                    cb.AddImage(signatureImage);
                }*/

                document.Close();
            }
        }

        // Список нарушений
        public static void GenerateViolationsReport(string outputPath, int inspectionId, string employeeFullname)
        {
            Inspection inspection = App.DbContext.Inspections.Single(r => r.InspectionId == inspectionId);

            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var document = new Document(PageSize.A4, 85f, 42f, 57f, 57f))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont boldFont = BaseFont.CreateFont("c:\\windows\\fonts\\timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                Font russianFont = new Font(baseFont, 10f, Font.ITALIC);
                Font headerFont = new Font(boldFont, 16f, Font.NORMAL);
                Font subHeaderFont = new Font(boldFont, 14f, Font.NORMAL);
                Font normalFont = new Font(baseFont, 12f, Font.NORMAL);
                Font boldNormalFont = new Font(boldFont, 12f, Font.NORMAL);

                Image logo = null;
                var uri = new Uri("/CarAccountingGibdd;component/Resources/LogoGibdd.png", UriKind.Relative);
                var streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        logo = Image.GetInstance(imageBytes);
                        logo.ScaleToFit(55f, 55f);
                        logo.Alignment = Image.ALIGN_LEFT;
                    }
                }

                // Создаем таблицу с 2 колонками для логотипа и названия
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.TotalWidth = 260f;
                headerTable.LockedWidth = true;
                headerTable.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTable.SetWidths(new float[] { 1f, 4f }); // ширина колонок (логотип меньше)

                // Левая ячейка - логотип
                PdfPCell logoCell = new PdfPCell();
                if (logo != null)
                {
                    logoCell.AddElement(logo);
                }
                logoCell.Border = PdfPCell.NO_BORDER;
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerTable.AddCell(logoCell);

                // Правая ячейка - название организации
                PdfPCell orgNameCell = new PdfPCell(new Phrase(14f, "Департамент Государственной инспекции безопасности дорожного движения\n(ГИБДД)", boldNormalFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingBottom = 7f,
                    PaddingLeft = 15f
                };
                headerTable.AddCell(orgNameCell);

                // Добавляем таблицу в документ
                document.Add(headerTable);

                // Заголовок
                Paragraph title = new Paragraph("Отчёт о выявленных нарушениях", headerFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 20f,
                    SpacingAfter = 15f
                };
                document.Add(title);

                // Номер отчета и дата выдачи
                Paragraph certInfo = new Paragraph(
                $"№ {inspection.InspectorId}\nДата выдачи: {inspection.DatetimeCompleted:dd.MM.yyyy}",
                    subHeaderFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(certInfo);

                // Информация о владельце и ТС
                var owner = inspection.Application.Owner;
                var vehicle = inspection.Application.Vehicle;
                Paragraph ownerHeader = new Paragraph("Информация о владельце и ТС:", boldNormalFont) { SpacingAfter = 5f };
                document.Add(ownerHeader);

                Paragraph ownerInfo = new Paragraph(
                    $"ФИО: {owner.Fullname}\n" +
                    $"Телефон: {owner.Phone}\n" +
                    $"Эл. почта: {owner.EmailValue}\n" +
                    $"ТС: {vehicle.Info}\n" +
                    $"VIN: {vehicle.Vin}",
                    normalFont)
                {
                    SpacingAfter = 15f
                };
                document.Add(ownerInfo);

                // Информация об инспекции
                Paragraph inspectionHeader = new Paragraph("Информация об инспекции:", boldNormalFont) { SpacingAfter = 5f };
                document.Add(inspectionHeader);

                Paragraph inspectionInfo = new Paragraph(
                    $"Номер заявки: {inspection.ApplicationId}\n" +
                    $"Номер инспекции: {inspection.InspectionId}\n" +
                    $"Дата и время: {inspection.DatetimePlanned:dd.MM.yyyy HH:mm}\n" +
                    $"Инспектор: {inspection.Inspector.Fullname}\n" +
                    $"Департамент: {inspection.Application.DepartmentName}",
                    normalFont)
                {
                    SpacingAfter = 15f
                };
                document.Add(inspectionInfo);

                // Список нарушений
                Paragraph violationsHeader = new Paragraph("Выявленные нарушения:", boldNormalFont) { SpacingAfter = 5f };
                document.Add(violationsHeader);

                // Используем iTextSharp.text.List для списка нарушений
                var violationsList = new iTextSharp.text.List(true, 20f); // true — нумерованный список
                foreach (var violationInspection in inspection.ViolationsInspections)
                {
                    var violation = violationInspection.Violation;
                    var listItem = new ListItem(violation.NumberDescription, normalFont);
                    violationsList.Add(listItem);
                }
                document.Add(violationsList);

                // Статус свидетельства
                Paragraph countViolations = new Paragraph($"Всего нарушений: {inspection.ViolationsInspections.Count()}", boldNormalFont)
                {
                    SpacingAfter = 20f,
                    SpacingBefore = 15f
                };
                document.Add(countViolations);

                // Дата составления и ответственный
                Paragraph footer = new Paragraph(
                    $"Дата составления отчета: {DateTime.Now:dd.MM.yyyy}\n" +
                    $"Ответственный сотрудник: {employeeFullname}",
                    normalFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                };
                document.Add(footer);

                // Добавляем таблицу с линией и надписью 
                PdfPTable signatureLineTable = new PdfPTable(1);
                signatureLineTable.TotalWidth = 200f;
                signatureLineTable.LockedWidth = true;
                signatureLineTable.HorizontalAlignment = Element.ALIGN_RIGHT;

                PdfPCell lineCell = new PdfPCell()
                {
                    BorderWidthBottom = 1f,
                    Border = PdfPCell.BOTTOM_BORDER,
                    FixedHeight = 20f,
                    Padding = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_BOTTOM
                };
                signatureLineTable.AddCell(lineCell);

                PdfPCell captionCell = new PdfPCell(new Phrase("(подпись)", russianFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 2f
                };
                signatureLineTable.AddCell(captionCell);

                document.Add(signatureLineTable);

                // Получаем текущую вертикальную позицию (нижний край таблицы)
                float yPos = writer.GetVerticalPosition(true);

                // Вычисляем координаты для изображения подписи
                float xPos = document.PageSize.Width - document.RightMargin - 200f + 40f; // подгоните под ширину таблицы и отступы
                float yImage = yPos + 5f; // немного выше линии, чтобы наложиться

                /*// Загружаем изображение подписи
                Image signatureImage = null;
                uri = new Uri("/CarAccountingGibdd;component/Resources/Signature.png", UriKind.Relative);
                streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        signatureImage = Image.GetInstance(imageBytes);
                        signatureImage.ScaleToFit(114f, 114f);
                    }
                }

                if (signatureImage != null)
                {
                    signatureImage.SetAbsolutePosition(xPos, yImage);
                    PdfContentByte cb = writer.DirectContent;
                    PdfGState graphicsState = new PdfGState();
                    graphicsState.FillOpacity = 0.7f;  // 50% прозрачность
                    cb.SetGState(graphicsState);
                    cb.AddImage(signatureImage);
                }*/

                document.Close();
            }
        }

        // Отчет PDF
        public static void GeneratePdfReport(string outputPath, List<Report> reports, DateOnly startDate, DateOnly endDate, string employeeFullname)
        {
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var document = new Document(PageSize.A4, 85f, 42f, 57f, 57f))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont boldFont = BaseFont.CreateFont("c:\\windows\\fonts\\timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                Font russianFont = new Font(baseFont, 10f, Font.ITALIC);
                Font headerFont = new Font(boldFont, 16f, Font.NORMAL);
                Font subHeaderFont = new Font(boldFont, 14f, Font.NORMAL);
                Font normalFont = new Font(baseFont, 12f, Font.NORMAL);
                Font boldNormalFont = new Font(boldFont, 12f, Font.NORMAL);
                Font boldHeaderTableFont = new Font(boldFont, 10f, Font.NORMAL);
                Font cellFont = new Font(baseFont, 10f, Font.NORMAL);

                // Логотип
                Image logo = null;
                var uri = new Uri("/CarAccountingGibdd;component/Resources/LogoGibdd.png", UriKind.Relative);
                var streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        logo = Image.GetInstance(imageBytes);
                        logo.ScaleToFit(55f, 55f);
                        logo.Alignment = Image.ALIGN_LEFT;
                    }
                }

                // Шапка с логотипом и названием
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.TotalWidth = 260f;
                headerTable.LockedWidth = true;
                headerTable.HorizontalAlignment = Element.ALIGN_LEFT;
                headerTable.SetWidths(new float[] { 1f, 4f });

                PdfPCell logoCell = new PdfPCell();
                if (logo != null)
                {
                    logoCell.AddElement(logo);
                }
                logoCell.Border = PdfPCell.NO_BORDER;
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerTable.AddCell(logoCell);

                PdfPCell orgNameCell = new PdfPCell(new Phrase(14f, "Департамент Государственной инспекции безопасности дорожного движения\n(ГИБДД)", boldNormalFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    PaddingBottom = 7f,
                    PaddingLeft = 15f
                };
                headerTable.AddCell(orgNameCell);

                document.Add(headerTable);

                // Название документа
                Paragraph title = new Paragraph($"Отчет по заявлениям за период с {startDate} по {endDate}", headerFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 20f,
                    SpacingAfter = 35f
                };
                document.Add(title);

                // Таблица с отчетными данными
                PdfPTable table = new PdfPTable(7); // 7 колонок
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1.4f, 1.5f, 1.7f, 2f, 1.7f, 1.7f, 1.5f });

                // Заголовки столбцов
                string[] headers = { "№ заявки", "Департамент", "Владелец", "Данные ТС", "Дата подачи", "Дата рассмотрения", "Статус" };
                foreach (var h in headers)
                {
                    var cell = new PdfPCell(new Phrase(h, boldHeaderTableFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BackgroundColor = new BaseColor(230, 230, 230),
                        Padding = 4f
                    };
                    table.AddCell(cell);
                }

                // Данные
                foreach (var r in reports)
                {
                    table.AddCell(new PdfPCell(new Phrase(r.ApplcationId.ToString(), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(r.DepartmentName, cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(r.OwnerFullname, cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(r.VehicleFullInfo, cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(r.DatetimeSupply.ToString("dd.MM.yyyy HH:mm"), cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(r.DatetimeConfirmValue, cellFont)));
                    table.AddCell(new PdfPCell(new Phrase(r.StatusName, cellFont)));
                }

                document.Add(table);

                // Данные по заявкам
                Paragraph inspectionHeader = new Paragraph("Данные о заявках:", boldNormalFont) { SpacingAfter = 5f, SpacingBefore = 15f };
                document.Add(inspectionHeader);

                Paragraph info = new Paragraph(
                    $"В работе: {reports.Where(r => r.StatusName != "Отклонена" && r.StatusName != "Требует доработки" && r.StatusName != "Завершена").Count()} заявок(-ки)\n" +
                    $"Отклонено: {reports.Where(r => r.StatusName == "Отклонена").Count()} заявок(-ки)\n" +
                    $"Имеют нарушения: {reports.Where(r => r.StatusName == "Требует доработки").Count()} заявок(-ки)\n" +
                    $"Выдано свидетельств: {reports.Where(r => r.StatusName == "Завершена").Count()} шт.\n",
                    normalFont)
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingAfter = 20f,
                };
                document.Add(info);

                // Всего заявок
                Paragraph countApplications = new Paragraph($"Всего поступило: {reports.Count()} заявок(-ки)", boldNormalFont)
                {
                    SpacingAfter = 20f,
                };
                document.Add(countApplications);

                // Дата составления и ответственный
                Paragraph footer = new Paragraph(
                    $"Дата составления отчета: {DateTime.Now:dd.MM.yyyy}\n" +
                    $"Ответственный сотрудник: {employeeFullname}",
                    normalFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                };
                document.Add(footer);

                // Добавляем таблицу с линией и надписью 
                PdfPTable signatureLineTable = new PdfPTable(1);
                signatureLineTable.TotalWidth = 200f;
                signatureLineTable.LockedWidth = true;
                signatureLineTable.HorizontalAlignment = Element.ALIGN_RIGHT;

                PdfPCell lineCell = new PdfPCell()
                {
                    BorderWidthBottom = 1f,
                    Border = PdfPCell.BOTTOM_BORDER,
                    FixedHeight = 20f,
                    Padding = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_BOTTOM
                };
                signatureLineTable.AddCell(lineCell);

                PdfPCell captionCell = new PdfPCell(new Phrase("(подпись)", russianFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 2f
                };
                signatureLineTable.AddCell(captionCell);

                document.Add(signatureLineTable);

                // Получаем текущую вертикальную позицию (нижний край таблицы)
                float yPos = writer.GetVerticalPosition(true);

                // Вычисляем координаты для изображения подписи
                float xPos = document.PageSize.Width - document.RightMargin - 200f + 40f; // подгоните под ширину таблицы и отступы
                float yImage = yPos + 5f; // немного выше линии, чтобы наложиться

                // Загружаем изображение подписи
                /*Image signatureImage = null;
                uri = new Uri("/CarAccountingGibdd;component/Resources/Signature.png", UriKind.Relative);
                streamResourceInfo = System.Windows.Application.GetResourceStream(uri);
                if (streamResourceInfo != null)
                {
                    using (var stream = streamResourceInfo.Stream)
                    {
                        byte[] imageBytes = new byte[stream.Length];
                        stream.Read(imageBytes, 0, imageBytes.Length);
                        signatureImage = Image.GetInstance(imageBytes);
                        signatureImage.ScaleToFit(114f, 114f);
                    }
                }

                if (signatureImage != null)
                {
                    signatureImage.SetAbsolutePosition(xPos, yImage);
                    PdfContentByte cb = writer.DirectContent;
                    PdfGState graphicsState = new PdfGState();
                    graphicsState.FillOpacity = 0.7f;  // 50% прозрачность
                    cb.SetGState(graphicsState);
                    cb.AddImage(signatureImage);
                }*/

                document.Close();
            }
        }

        // Отчет Excel по показателям и значениям
        public static void GenerateExcelReport(
            string outputPath,
            List<ReportItem> reportItems,
            DateOnly startDate,
            DateOnly endDate,
            string employeeFullname,
            string organizationName = "Департамент Государственной инспекции безопасности дорожного движения\n(ГИБДД)")
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Отчёт");
                int row = 1;

                // --- Заголовок ---
                var headerCell = ws.Cell(row, 1);
                headerCell.Value = organizationName;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Font.FontSize = 16;
                headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                row += 2;

                // --- Период отчёта ---
                ws.Cell(row, 1).Value = "Период отчёта:";
                ws.Range(row, 2, row, 2).Merge();
                ws.Cell(row, 2).Value = $"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
                row += 2;

                // --- Таблица показателей ---
                ws.Cell(row, 1).Value = "Показатель";
                ws.Cell(row, 2).Value = "Значение";
                ws.Range(row, 1, row, 2).Style.Font.Bold = true;
                ws.Range(row, 1, row, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                row++;

                foreach (var item in reportItems)
                {
                    ws.Cell(row, 2).Value = item.Value;
                    row++;
                }

                row++;

                // --- Информация о сотруднике и дате ---
                ws.Cell(row, 1).Value = "Составил:";
                ws.Cell(row, 2).Value = employeeFullname;
                row++;
                ws.Cell(row, 1).Value = "Дата составления:";
                ws.Cell(row, 2).Value = DateTime.Now.ToString("dd.MM.yyyy");

                // --- Оформление ---
                ws.Columns().AdjustToContents();

                // Сохраняем файл с обработкой ошибок
                bool saved = false;
                while (!saved)
                {
                    try
                    {
                        workbook.SaveAs(outputPath);
                        saved = true;
                    }
                    catch
                    {
                        var result = MessageBox.Show(
                            "Файл уже открыт в другой программе. Пожалуйста, закройте его и нажмите 'OK' для повторной попытки, или 'Отмена', чтобы отобразить открытый файл.",
                            "Файл занят",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Cancel)
                            break;
                    }
                }
            }
        }
    }
}
