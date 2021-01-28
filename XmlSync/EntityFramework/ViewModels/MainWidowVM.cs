﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XmlSync.EntityFramework.Models;
using XmlSync.Properties;

namespace XmlSync.EntityFramework.ViewModels
{
    public class MainWidowVM : ViewModelBase
    {

        public MainWidowVM()
        {
            _context.Database.Migrate();
        }

        DirectoryInfo Dir = new DirectoryInfo(Settings.Default.Caminho);
        XmlSyncDbContext _context = new XmlSyncFactory().CreateDbContext();
       
        XML_INFO xml_info = new XML_INFO();
        public void IniciarServicos()
        {

            BuscaXmlPasta();


        }

       // public async Task<string> BuscaXmlPasta()
       public void BuscaXmlPasta()
        {
            try
            {
                
                //for (int i = 0; i < 10; i++)
                //{

                  
                   // xml = _context.XML_INFOs.Select(c => c).Where(c => c.SINCRONIZADO == false).ToList();
                   foreach(FileInfo info in Dir.GetFiles())                    
                   {
                        xml_info = _context.XML_INFOs.Select(c => c).Where(c => c.NOME_XML == info.Name).FirstOrDefault();
                        //if (xml_info?.NOME_XML is not null) 
                        //{
                            TextReader Leitor = (TextReader)new StreamReader(info.FullName);
                            XML_INFO X = new XML_INFO();
                            X.NOME_XML = info.Name;
                            X.CONTEUDO = Leitor.ReadToEnd();
                            X.DATA_CRIAÇÃO = info.CreationTime;
                            X.SINCRONIZADO = false;
                            SincronizarFTP(info);
                            _context.Update(X);
                            _context.SaveChanges();

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
                //return "deu ruim as " + DateTime.Now;
                //throw;


            }//a

        }
        public void SincronizarFTP(FileInfo info) 
        {
            try
            {
                 List<XML_INFO> xml = new List<XML_INFO>();

                 xml = _context.XML_INFOs.Select(c => c).Where(c => c.SINCRONIZADO == false).ToList();


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
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}