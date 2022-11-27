using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Front_End_Eleicoes_Goias
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        SqlConnection con;

        protected void AbrirConexao()
        {
            string connectionString = "Data Source=;Initial Catalog=;User ID=;Password=;";
            con = new SqlConnection(connectionString);
            con.Open();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AbrirConexao();
                if (!IsPostBack)
                {
                    Listar_Municipios();
                }
            }
            catch (Exception d)
            {
                Response.Write($"<script>alert('{d.Message}')</script>");
            }
        }

        protected void Listar_Municipios()
        {
            try
            {
                string comando = "SELECT NM_MUNICIPIO FROM Municipio ORDER BY NM_MUNICIPIO";
                SqlCommand command = new SqlCommand(comando, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Municipios.Items.Add(reader[0].ToString());
                }
                reader.Close();
            }
            catch (Exception d)
            {
                Response.Write($"<script>alert({d.Message})</script>");
            }
            
        }

        protected void InsereItemsNaDDL(SqlDataReader reader, DropDownList ddl, string campo)
        {
            ddl.DataSource = reader;
            ddl.DataTextField = campo;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("", "")); //insere um item antes, deixando a caixa em branco

            reader.Close();
        }

        protected void Municipios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Secao.Items.Clear();
                Secao.Items.Insert(0, new ListItem("", ""));
                Urna.Items.Clear();
                Urna.Items.Insert(0, new ListItem("", ""));
                string comando = "SELECT DISTINCT NR_ZONA FROM Secao s " +
                 "INNER JOIN Municipio m ON s.CD_MUNICIPIO = m.CD_MUNICIPIO " +
                 "WHERE NM_MUNICIPIO = @Municipio ORDER BY NR_ZONA";
                SqlCommand command = new SqlCommand(comando, con);
                command.Parameters.AddWithValue("@Municipio", Municipios.SelectedValue);
                SqlDataReader reader = command.ExecuteReader();
                InsereItemsNaDDL(reader, Zona, "NR_ZONA");
                reader.Close();
            }
            catch(Exception d)
            {
                Response.Write($"<script>alert('{d.Message}')</script>");
            }

        }

        protected void Zona_SelectedIndexChanged(object sender, EventArgs e)
        {
            Urna.Items.Clear();
            Urna.Items.Insert(0, new ListItem("", ""));
            string comando = "SELECT NR_SECAO FROM Secao s " +
                 "INNER JOIN Municipio m ON s.CD_MUNICIPIO = m.CD_MUNICIPIO " +
                 "WHERE NM_MUNICIPIO = @Municipio and NR_ZONA = @Zona ORDER BY NR_SECAO";
            SqlCommand command = new SqlCommand(comando, con);
            command.Parameters.AddWithValue("@Municipio", Municipios.SelectedValue);
            command.Parameters.AddWithValue("@Zona", Zona.SelectedValue);
            SqlDataReader reader = command.ExecuteReader();
            InsereItemsNaDDL(reader, Secao, "NR_SECAO");

            reader.Close();
        }

        protected void Secao_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comando = "SELECT NR_URNA_EFETIVADA FROM Urna u " +
                             "INNER JOIN Secao s ON u.NR_ZONA = s.NR_ZONA AND u.NR_SECAO = s.NR_SECAO " +
                             "WHERE s.NR_ZONA = @Zona and s.NR_SECAO = @Secao ORDER BY u.NR_URNA_EFETIVADA";
            SqlCommand command = new SqlCommand(comando, con);
            command.Parameters.AddWithValue("@Secao", Secao.SelectedValue);
            command.Parameters.AddWithValue("@Zona", Zona.SelectedValue);
            SqlDataReader reader = command.ExecuteReader();
            InsereItemsNaDDL(reader, Urna, "NR_URNA_EFETIVADA");

            reader.Close();
        }

        protected void PreencherDadosSecao()
        {
            string comando = "SELECT S.NR_ZONA, S.NR_SECAO, M.NM_MUNICIPIO, S.QT_APTOS, S.QT_COMPARECIMENTO, S.QT_ABSTENCOES, " +
                             "S.QT_ELEITORES_BIOMETRIA_NH, S.DS_AGREGADAS FROM SECAO S INNER JOIN MUNICIPIO M ON S.CD_MUNICIPIO = " +
                             "M.CD_MUNICIPIO WHERE S.NR_ZONA = @ZONA AND S.NR_SECAO = @Secao";
            AbrirConexao();
            SqlCommand command = new SqlCommand(comando, con);
            command.Parameters.AddWithValue("@Zona", Zona.SelectedValue);
            command.Parameters.AddWithValue("@Secao", Secao.SelectedValue);
            SqlDataReader reader = command.ExecuteReader();
            detalhe_secao_pai.Visible = true;

            while (reader.Read())
            {
                lb_nr_zona.Text = "Número da zona: " + reader[0].ToString();
                lb_nr_secao.Text = "Número da seção: " + reader[1].ToString();
                lb_nm_municipio.Text = "Nome do município: " + reader[2].ToString();
                lb_qt_aptos.Text = "Número de pessoas aptas a votar: " + reader[3].ToString();
                lb_qt_comparecimento.Text = "Número de pessoas que compareceram: " + reader[4].ToString();
                lb_qt_abstencoes.Text = "Número de abstenções: " + reader[5].ToString();
                lb_qt_eleitores_biometria_nh.Text = "Número de pessoas com biometria: " + reader[6].ToString();
                lb_ds_agregadas.Text = String.IsNullOrEmpty(reader[7].ToString()) ? "Existe zona agregada: Não" : "Existe zona agregada: Sim, número da seção: " + reader[7].ToString();
            }

        }

        protected void PreencherDadosUrna()
        {
            string comando = "SELECT U.NR_URNA_EFETIVADA, U.CD_FLASHCARD_URNA_EFETIVADA, U.DT_EMISSAO_BU, " +
                             "U.DT_BU_RECEBIDO, U.DT_CARGA_URNA_EFETIVADA, TU.DS_TIPO_URNA FROM URNA U INNER JOIN " +
                             "TIPO_URNA TU ON U.CD_TIPO_URNA = TU.CD_TIPO_URNA WHERE U.NR_SECAO = @Secao and U.NR_ZONA = @Zona and U.NR_URNA_EFETIVADA = @Urna";
            AbrirConexao();
            SqlCommand command = new SqlCommand(comando, con);
            command.Parameters.AddWithValue("@Zona", Zona.SelectedValue);
            command.Parameters.AddWithValue("@Secao", Secao.SelectedValue);
            command.Parameters.AddWithValue("@Urna", Urna.SelectedValue);
            SqlDataReader reader = command.ExecuteReader();
            imagem_urna.Visible = true;

            while (reader.Read())
            {
                lb_nr_urna_efetivada.Text = "Código da urna: " + reader[0].ToString();
                lb_cd_flashcard_urna_efetivada.Text = "Flashcard da urna: " + reader[1].ToString();
                lb_dt_emissao_bu.Text = "Emissão do boletim de urna " + reader[2].ToString();
                lb_dt_bu_recebido.Text = "Recebimento do boletim de urna: " + reader[3].ToString();
                lb_dt_carga_urna_efetivada.Text = "Carregamento dos dados da urna: " + reader[4].ToString();
                lb_ds_tipo_urna.Text = "Estado da urna: " + reader[5].ToString();
            }

            reader.Close();
        }

        protected bool VerificarCaixasVazias()
        {
            string[] campos = { Municipios.SelectedValue, Zona.SelectedValue,
                               Secao.SelectedValue, Cargo.SelectedValue, Urna.SelectedValue};

            foreach(string s in campos)
            {
                if (String.IsNullOrEmpty(s))
                    return true;
            }
            
            return false;
        }

        protected void Buscar_OnClick(object sender, EventArgs e)
        {
            if (VerificarCaixasVazias())
            {
                Response.Write($"<script>alert('Existem caixas vazias que devem ser preenchidas!')</script>");
                return;
            }

            try
            {
                AbrirConexao();
                string comando = "SELECT V.NR_VOTAVEL, TC.DS_CARGO_PERGUNTA, C.NM_VOTAVEL, P.SG_PARTIDO, P.NM_PARTIDO, V.QT_VOTOS FROM VOTOS V INNER JOIN TIPO_CARGO TC " +
                                "ON TC.CD_CARGO_PERGUNTA = V.CD_CARGO_PERGUNTA INNER JOIN  CANDIDATO C ON C.NR_VOTAVEL = V.NR_VOTAVEL AND C.CD_CARGO_PERGUNTA = V.CD_CARGO_PERGUNTA " +
                                "INNER JOIN PARTIDO P ON P.NR_PARTIDO = C.NR_PARTIDO WHERE NR_URNA_EFETIVADA = @Urna AND TC.DS_CARGO_PERGUNTA = @Cargo " +
                                "AND NOT C.CD_TIPO_VOTAVEL = 4 ORDER BY V.NR_VOTAVEL";
                SqlCommand command = new SqlCommand(comando, con);
                command.Parameters.AddWithValue("@Urna", Urna.SelectedValue);
                command.Parameters.AddWithValue("@Cargo", Cargo.SelectedValue);
                SqlDataReader reader = command.ExecuteReader();
                StringBuilder sb = new StringBuilder();

                while (reader.Read())
                {
                    sb.AppendLine("<div class=\"informacao_candidato\">");
                    sb.AppendLine($"<h5>Número do candidato: {reader[0].ToString()}</h5>");
                    sb.AppendLine($"<h5>Cargo concorrido: {reader[1].ToString()}</h5>");
                    sb.AppendLine($"<h5>Nome do candidato: {reader[2].ToString()}</h5>");
                    sb.AppendLine($"<h5>Partido: {reader[3].ToString()}</h5>");
                    sb.AppendLine($"<h5>Nome do Partido: {reader[4].ToString()}</h5>");
                    sb.AppendLine($"<h5>Quantidade de votos recebidos: {reader[5].ToString()}</h5>");
                    sb.AppendLine("</div>");
                }

                reader.Close();

                lt_detalhe_voto.Text = sb.ToString();

                PreencherDadosSecao();
                PreencherDadosUrna();
            }
            catch(Exception d)
            {
                Response.Write($"<script>alert('{d.Message}')</script>");
            }

        }
    }
}