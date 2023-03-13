namespace WebServiceClient
{
    public partial class Form1 : Form {

        private readonly HttpRequestHandler requestHandler = new HttpRequestHandler(new HttpClient());

     

        List<Film> partialFilms = new List<Film>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void InitializeView(List<Film> films)
        {
            comboBox1.Items.Clear();
            partialFilms = films;
            for (int i = 0; i < films.Count; i++)
            {
                Film film = films[i];
                comboBox1.Items.Add(film.Title);
            }

            if (films.Count > 0) {

                comboBox1.SelectedIndex = 0;
                Film selectedFilm = await requestHandler.GetFilm(films[0].imdbID);
                if (selectedFilm == null) return;

                label3.Text = selectedFilm.Title;
                label4.Text = selectedFilm.Genre;
                label7.Text = selectedFilm.Runtime;
                label9.Text = selectedFilm.imdbRating;
                label11.Text = selectedFilm.Released;
                try { pictureBox1.LoadAsync(selectedFilm.Poster); }
                catch (Exception) {
                    pictureBox1.LoadAsync("https://eclipsys.ca/wp-content/uploads/2022/03/Error-404.png");
                }
            } else {
                MessageBox.Show("Nessun film trovato per chiave di ricerca data.");
                label3.Text = "N/A";
                label4.Text = "N/A";
                label7.Text = "N/A";
                label9.Text = "N/A";
                label11.Text = "N/A";
                pictureBox1.LoadAsync("https://eclipsys.ca/wp-content/uploads/2022/03/Error-404.png");
            }

        }

        private async void button1_Click(object sender, EventArgs e) {

            List<Film> filmList = await requestHandler.GetAllFilm(textBox1.Text);
            InitializeView(filmList);
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

            if (!comboBox1.Focused) return;

            Film selectedFilm = await requestHandler.GetFilm(partialFilms[comboBox1.SelectedIndex].imdbID);
            if (selectedFilm == null) return;

            label3.Text = selectedFilm.Title;
            label4.Text = selectedFilm.Genre;
            label7.Text = selectedFilm.Runtime;
            label9.Text = selectedFilm.imdbRating;
            label11.Text = selectedFilm.Released;
            try { pictureBox1.LoadAsync(selectedFilm.Poster); }
            catch (Exception) {
                pictureBox1.LoadAsync("https://eclipsys.ca/wp-content/uploads/2022/03/Error-404.png");
            }
        }

        private async void button2_Click(object sender, EventArgs e) {
            List<Film> filmList = await requestHandler.GetUpdatePage(textBox1.Text);
            InitializeView(filmList);
        }

        private async void button3_Click(object sender, EventArgs e) {
            List<Film> filmList = await requestHandler.GoPreviousPage(textBox1.Text);
            InitializeView(filmList);
        }
    }
}