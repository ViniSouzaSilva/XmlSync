﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XmlSync.EntityFramework.ViewModels;
using XmlSync.Properties;

namespace XmlSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {//a
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWidowVM();
            Login_txb.Text = Settings.Default.Login;
            Senha_txb.Text = Settings.Default.Senha;
            Caminho_txb.Text = Settings.Default.Caminho;
            CaminhoFTP_txb.Text = Settings.Default.CaminhoFTP;

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NomeArquivo_txb.Text.Equals(""))
            {
                MessageBox.Show("Digite um nome para o arquivo","Atenção",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else 
            {
                Iniciar_lbl.Content = "";
                ((MainWidowVM)DataContext).BuscaXmlPasta(NomeArquivo_txb.Text);
                Iniciar_lbl.Content = "Upload feito com sucesso";

            }
        }

        private void Salvar_btn_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Login = Login_txb.Text;
            Settings.Default.Senha = Senha_txb.Text;
            Settings.Default.Caminho = Caminho_txb.Text;
            Settings.Default.CaminhoFTP = CaminhoFTP_txb.Text;
            Settings.Default.Save();
        }
    }
}
