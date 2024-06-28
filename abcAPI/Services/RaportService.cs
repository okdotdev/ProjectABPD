using abcAPI.Models.DTOs;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace abcAPI.Services;

public class RaportService : IRaportService
{
    public async Task<byte[]> GeneratePdfReportAsync(List<GetContractDto> contracts)
    {
        using MemoryStream ms = new MemoryStream();
        Document document = new Document(PageSize.A4, 50, 50, 25, 25);
        PdfWriter writer = PdfWriter.GetInstance(document, ms);
        document.Open();


        Font dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);
        Paragraph date = new($"Date: {DateTime.Now.ToShortDateString()}", dateFont)
        {
            Alignment = Element.ALIGN_RIGHT,
            SpacingAfter = 20
        };
        document.Add(date);


        Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
        Paragraph title = new Paragraph("Contracts Report", titleFont)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 20
        };
        document.Add(title);


        PdfPTable table = new(9)
        {
            WidthPercentage = 100
        };
        table.SetWidths([1.5f, 2.5f, 2.5f, 2.5f, 2.5f, 1.5f, 1.5f, 2.5f, 1.5f]);

        // Add Table Header
        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
        AddCellToHeader(table, "Contract ID", headerFont);
        AddCellToHeader(table, "Price", headerFont);
        AddCellToHeader(table, "Amount Paid", headerFont);
        AddCellToHeader(table, "Start Date", headerFont);
        AddCellToHeader(table, "End Date", headerFont);
        AddCellToHeader(table, "Is Paid", headerFont);
        AddCellToHeader(table, "Version", headerFont);
        AddCellToHeader(table, "Support Years", headerFont);
        AddCellToHeader(table, "Is Signed", headerFont);

        // Add Data Rows
        Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
        foreach (var contract in contracts)
        {
            AddCellToBody(table, contract.Id.ToString(), cellFont);
            AddCellToBody(table, contract.Price.ToString("C"), cellFont);
            AddCellToBody(table, contract.AmountPaid.ToString("C"), cellFont);
            AddCellToBody(table, contract.StartDate.ToShortDateString(), cellFont);
            AddCellToBody(table, contract.EndDate.ToShortDateString(), cellFont);
            AddCellToBody(table, contract.IsPaid ? "Yes" : "No", cellFont);
            AddCellToBody(table, contract.Version, cellFont);
            AddCellToBody(table, contract.AdditionalSupportYears.ToString(), cellFont);
            AddCellToBody(table, contract.IsSigned ? "Yes" : "No", cellFont);
        }

        document.Add(table);
        document.Close();
        writer.Close();

        return ms.ToArray();
    }

    private void AddCellToHeader(PdfPTable table, string text, Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font))
        {
            BackgroundColor = BaseColor.BLUE,
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 5
        };
        table.AddCell(cell);
    }

    private void AddCellToBody(PdfPTable table, string text, Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font))
        {
            HorizontalAlignment = Element.ALIGN_CENTER,
            Padding = 5
        };
        table.AddCell(cell);
    }


}