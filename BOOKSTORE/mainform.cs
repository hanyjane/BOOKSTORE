using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOOKSTORE
{
    public partial class mainform : Form
    {
        public mainform()
        {
            InitializeComponent();
            this.Load += new EventHandler(mainform_Load);
        }

        private void mainform_Load(object sender, EventArgs e)
        {
            var books = LoadBooksFromDatabase();
            foreach (var book in books)
            {
                AddBookToUI(book);
            }
            
        }


        private List<Book> LoadBooksFromDatabase()
        {
            List<Book> books = new List<Book>();

            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\reyneil\Desktop\Database11.accdb"))
            {
                conn.Open();
                string query = "SELECT ID, Category, Title, ISBN, Author, Stock, Price, BookCover FROM Books";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Category = reader.GetString(1),
                            Title = reader.GetString(2),
                            ISBN = reader.GetString(3),
                            Author = reader.GetString(4),
                            Stock = reader.GetInt32(5),
                            Price = reader.GetDecimal(6),
                            BookCover = reader.IsDBNull(7) ? null : (byte[])reader[7]
                        });
                    }
                }
            }

            return books;
        }

        private void AddBookToUI(Book book)
        {
            Panel panel = new Panel
            {
                Size = new Size(150, 270),
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox picBox = new PictureBox
            {
                Size = new Size(110, 130),
                Location = new Point(20, 10),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            if (book.BookCover != null)
            {
                using (MemoryStream ms = new MemoryStream(book.BookCover))
                {
                    picBox.Image = Image.FromStream(ms);
                }
            }

            Label lblTitle = new Label
            {
                Text = book.Title,
                Location = new Point(10, 150),
                AutoSize = true
            };

            Label lblAuthor = new Label
            {
                Text = "By " + book.Author,
                Location = new Point(10, 170),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.DarkGray
            };

            Label lblPrice = new Label
            {
                Text = "₱" + book.Price.ToString("0.00"),
                Location = new Point(10, 190),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            Label lblStock = new Label
            {
                Text = "Stock: " + book.Stock,
                Location = new Point(10, 210),
                AutoSize = true
            };

            panel.Controls.Add(picBox);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblAuthor);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(lblStock);
           

            flowLayoutPanel1.Controls.Add(panel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bookcreation bookcreationform = new Bookcreation(); // create instance of Register form
            bookcreationform.Show();                    // show the Register form
            this.Hide();
        }
    }

    // Book class (can be placed in a separate Book.cs file)
}
