using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XmlSync.EntityFramework.Models;
using XmlSync.Properties;
using System.IO.Compression;

namespace XmlSync.EntityFramework.ViewModels
{
    public class MainWidowVM : ViewModelBase
    {

        public MainWidowVM()
        {
            _context.Database.Migrate();
        }

        DirectoryInfo Dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "ZIP");
        DirectoryInfo Diretorio = new DirectoryInfo(Settings.Default.Caminho);
        DirectoryInfo DiretorioTemp = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Temp");
        XmlSyncDbContext _context = new XmlSyncFactory().CreateDbContext();
        DateTime MesAnterior = DateTime.Now.AddMonths(-1);
        XML_INFO xml_info = new XML_INFO();

        public void LimpaPastas() 
        {
            try
            {
                foreach (FileInfo info in DiretorioTemp.GetFiles())
                {
                    File.Delete(info.FullName);
                }
                foreach (FileInfo info in Dir.GetFiles())
                {
                    File.Delete(info.FullName);
                }
            }
            catch (Exception ex) 
            {
                audit("ERRO", ex.Message);
            }
        }


       public void SeparaXml() 
       {
            try
            {
                
                LimpaPastas();
                foreach (FileInfo info in Diretorio.GetFiles())
                {
                    if (info.LastWriteTime.Month == MesAnterior.Month && info.LastWriteTime.Year == MesAnterior.Year && info.Extension.ToLower() == ".xml")
                    {
                        info.CopyTo(AppDomain.CurrentDomain.BaseDirectory + "Temp\\"+info.Name);


                    }

                }
            }
            catch (Exception ex) 
            {

                audit("ERRO", ex.Message);

            }

       }
       // public async Task<string> BuscaXmlPasta()
       public void BuscaXmlPasta()
        {
            try
            {
                SeparaXml();
                Compressao();
                //for (int i = 0; i < 10; i++)
                //{


                // xml = _context.XML_INFOs.Select(c => c).Where(c => c.SINCRONIZADO == false).ToList();
                foreach (FileInfo info in Dir.GetFiles())                    
                   {
                    
                        //xml_info = _context.XML_INFOs.Select(c => c).Where(c => c.NOME_XML == info.Name).FirstOrDefault();
                        //if (xml_info?.NOME_XML is not null) 
                        //{
                           // TextReader Leitor = (TextReader)new StreamReader(info.FullName);
                           // XML_INFO X = new XML_INFO();
                          //  X.NOME_XML = info.Name;
                          //  X.CONTEUDO = Leitor.ReadToEnd();
                          //  X.DATA_CRIAÇÃO = info.CreationTime;
                          //  X.SINCRONIZADO = false;
                            SincronizarFTP(info);
                           // _context.Update(X);
                          // _context.SaveChanges();

                            //_context.XML_INFOs.Select(c=>c)

                        //}


                    }
                   
                    //await Task.Delay(TimeSpan.FromMilliseconds(300000));
                    //900000
                    //Thread.Sleep(5000);

                //}
                //return "";
            }
            catch (Exception ex)
            {
                audit("ERRO", ex.Message);
                //return "deu ruim as " + DateTime.Now;
                //throw;


            }//a

        }
        public void SincronizarFTP(FileInfo info) 
        {
            try
            {
                // List<XML_INFO> xml = new List<XML_INFO>();

                // xml = _context.XML_INFOs.Select(c => c).Where(c => c.SINCRONIZADO == false).ToList();


              //  foreach (XML_INFO _INFOs in xml)
                

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Settings.Default.CaminhoFTP +"/"+ info.Name));

                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(Settings.Default.Login, Settings.Default.Senha);

                    request.KeepAlive = false;

                    request.UseBinary = true;

                    request.ContentLength = info.Length;

                     //cria a stream que será usada para mandar o arquivo via FTP
                    Stream responseStream = request.GetRequestStream();
                    byte[] buffer = new byte[2048];


                    //Lê o arquivo de origem
                    FileStream fs = info.OpenRead();

                    //Enquanto vai lendo o arquivo de origem, vai escrevendo no FTP
                    int readCount = fs.Read(buffer, 0, buffer.Length);
                    while (readCount > 0)
                    {
                        //Esceve o arquivo
                        responseStream.Write(buffer, 0, readCount);
                        readCount = fs.Read(buffer, 0, buffer.Length);
                    }


                     fs.Close();
                     responseStream.Close();
                audit("Funcionou","The wide xml walk away");

            }
            catch (Exception ex)
            {
                audit("ERRO", ex.Message);
                
            }

        }

        public void Compressao()
        {
            try
            {
                ZipFile.CreateFromDirectory(AppDomain.CurrentDomain.BaseDirectory+"Temp", AppDomain.CurrentDomain.BaseDirectory+"ZIP"+"\\"+ MesAnterior.Month+MesAnterior.Year+".zip");
            }
            catch (Exception ex)
            {

                audit("ERRO", ex.Message);
            }
            

        }

        #region Auditoria 
        static StreamWriter auditwriter = new StreamWriter($@"{AppDomain.CurrentDomain.BaseDirectory}\Logs\audit-{DateTime.Today.ToString("dd-MM-yy")}.txt", true) { AutoFlush = true };
        readonly static object auditObj = new object();


        public static void audit(string tag, string texto, int nivelDeAuditoria = 1)
        {
            System.IO.Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\Logs");
            lock (auditObj)
            {
               
                    auditwriter.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t{tag} >>\t{texto}");
                
            }
        }//Escreve no arquivo da auditoria.
        #endregion
    }
}
