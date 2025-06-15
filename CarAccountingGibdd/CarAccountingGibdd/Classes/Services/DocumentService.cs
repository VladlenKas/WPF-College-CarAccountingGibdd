using CarAccountingGibdd.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CarAccountingGibdd.Classes.Services
{
    public class DocumentService
    {

        // Свидетельство
        public static void GenerateCertificate(string outputPath, Certificate certificate, string employeeFullname)
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
                    $"Департамент: {app.Department.Name}",
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

                // Загружаем изображение подписи
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
                }

                document.Close();
                // fs и document автоматически закроются и освободятся
            }
        }

        // Список нарушений
        public static void GenerateViolationsInspection(string outputPath)
        {

        }

        // Отчет
        public static void GenerateReport(string outputPath)
        {

        }
    }
}
