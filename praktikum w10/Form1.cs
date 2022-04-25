using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace praktikum_w10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string sqlConnection = "server=localhost;uid=root;pwd=;database=premier_league";
        public MySqlConnection sqlConnect = new MySqlConnection(sqlConnection); //data koneksi ke dbms
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;
        public string sqlQuery;

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable player = new DataTable();
            DataTable playerKanan = new DataTable();
            sqlQuery = "select team_name as 'Nama Tim', team_id as 'ID Team' from team";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(player);

            sqlQuery = "select team_name as 'Nama Tim', team_id as 'ID Team' from team";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(playerKanan);

            comboBoxKiri.DataSource = player;
            comboBoxKiri.DisplayMember = "Nama Tim";
            comboBoxKiri.ValueMember = "Nama Tim";

            comboBoxKanan.DataSource = playerKanan;
            comboBoxKanan.DisplayMember = "Nama Tim";
            comboBoxKanan.ValueMember = "Nama Tim";
        }

        private void comboBoxKiri_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtManager = new DataTable();
                sqlQuery = "select p.player_name , m.manager_name, concat(t.home_stadium, ',', t.city), t.capacity from team t, manager m, player p WHERE t.manager_id = m.manager_id and t.captain_id = p.player_id and team_name = '" + comboBoxKiri.SelectedValue + "'";
                sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
                sqlAdapter = new MySqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dtManager);
                labelManagerKiri.Text = dtManager.Rows[0][1].ToString();
                labelKaptenKiri.Text = dtManager.Rows[0][0].ToString();
                labelstadium.Text = dtManager.Rows[0][2].ToString();
                labelkapasitas.Text = dtManager.Rows[0][3].ToString();

            }
            catch (Exception)
            {


            }
        }

        private void comboBoxKanan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtManager = new DataTable();
                sqlQuery = "select p.player_name , m.manager_name, concat(t.home_stadium, ',', t.city), t.capacity from team t, manager m, player p WHERE t.manager_id = m.manager_id and t.captain_id = p.player_id and team_name = '" + comboBoxKanan.SelectedValue + "'";
                sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
                sqlAdapter = new MySqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(dtManager);
                labelManagerKanan.Text = dtManager.Rows[0][1].ToString();
                labelKaptenKanan.Text = dtManager.Rows[0][0].ToString();
            }
            catch (Exception)
            {


            }
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            DataTable dateScore = new DataTable();
            sqlQuery = "select date_format(match.match_date, '%e %M %Y'),concat(match.goal_home,' - ',match.goal_away)  from player p, dmatch d,team t, team t2, `match` where d.match_id = match.match_id and p.player_id = d.player_id and (((t.team_name = '" + comboBoxKiri.SelectedValue.ToString() + "'and t2.team_name = '" + comboBoxKanan.SelectedValue.ToString() + "')or (t2.team_name = '" + comboBoxKiri.SelectedValue.ToString() + "' and t.team_name = '" + comboBoxKanan.SelectedValue.ToString() + "')) and ((t.team_id = match.team_home and t2.team_id = match.team_away) or (t.team_id = match.team_away and t2.team_id = match.team_home) )); ";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dateScore);
            labelTanggal.Text = dateScore.Rows[0][0].ToString();
            labelSkor.Text = dateScore.Rows[0][1].ToString();
            DataTable tabel = new DataTable();
            sqlQuery = "select d.minute , if(p.team_id = match.team_home,p.player_name,' ') as 'player 1',if(p.team_id = match.team_home,if(d.type = 'CY' ,'Yellow Card',if(d.type = 'CR','Red Card',if(d.type = 'GO','Goal',if(d.type = 'GP','Goal Penalty',if(d.type = 'GW','Own Goal','Penalty Miss'))))),' ') as 'Type 1',if(p.team_id = match.team_away,p.player_name,' ') as 'player 2',if(p.team_id = match.team_away,if(d.type = 'CY' ,'Yellow Card',if(d.type = 'CR','Red Card',if(d.type = 'GO','Goal',if(d.type = 'GP','Goal Penalty',if(d.type = 'GW','Own Goal','Penalty Miss'))))),' ') as 'Type 2' from player p, dmatch d,team t, team t2, `match` where d.match_id = match.match_id and p.player_id = d.player_id and (((t.team_name = '" + comboBoxKiri.SelectedValue.ToString() + "'and t2.team_name = '" + comboBoxKanan.SelectedValue.ToString() + "')or (t2.team_name = '" + comboBoxKiri.SelectedValue.ToString() + "' and t.team_name = '" + comboBoxKanan.SelectedValue.ToString() + "')) and ((t.team_id = match.team_home and t2.team_id = match.team_away) or (t.team_id = match.team_away and t2.team_id = match.team_home) )) group by 1 order by 1; ";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(tabel);
            dataGridViewMatch.DataSource = tabel;
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void labelTanggal_Click(object sender, EventArgs e)
        {

        }

        private void labelSkor_Click(object sender, EventArgs e)
        {

        }
    }
}
