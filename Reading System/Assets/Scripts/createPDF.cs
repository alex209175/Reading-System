using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class createPDF : MonoBehaviour
{
    string[] emailAddresses; //email addresses in array
    string[] passwords; //passwords in array
    
    void Start() {
        Debug.Log(Application.persistentDataPath);
    }

    public void GeneratePDF()
    {
        emailAddresses = PlayerPrefs.GetString("emails").Split("\n"); //sets email address array
        passwords = PlayerPrefs.GetString("passwords").Split("\n"); //sets passwords array

        Document pdfDocument = new Document(); //creates a new pdf document
        PdfWriter.GetInstance(pdfDocument, new FileStream(Application.persistentDataPath + @"\easyReader_ClassList_" + PlayerPrefs.GetString("class") + ".pdf", FileMode.Create)); //sets file path location
        pdfDocument.Open();

        Chunk chunk = new Chunk("Class Code: " + PlayerPrefs.GetString("class") + "\n\n", FontFactory.GetFont("Helvetica", 20));
        Paragraph paragraph = new Paragraph();
        paragraph.Add(chunk);
        paragraph.Alignment = Element.ALIGN_CENTER; //centres paragraph
        pdfDocument.Add(paragraph);

        PdfPTable table = new PdfPTable(2); //creates table in document

        for (int i=0; i<emailAddresses.Length; i++) {
            table.AddCell(emailAddresses[i]); //adds email addresses and passwords to the pdf
            table.AddCell(passwords[i]);
        }

        table.HorizontalAlignment = Element.ALIGN_CENTER; //centres table

        pdfDocument.Add(table);

        pdfDocument.Close();

        Application.OpenURL(Application.persistentDataPath + @"\easyReader_ClassList_" + PlayerPrefs.GetString("class") + ".pdf"); //opens the file
    }
}
