using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Three_Layers.Core
{
    class UserDAL
    {
        public UserDAL()
        {
            
        }

        /// <summary>
        /// Получение списка клиентов из файла input.txt.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<Client> GetInfoFromFile()
        {
            const string bankPattern = @"Банк\:\s+[А-Яа-я0-9]+";
            const string clientPattern = @"Клиент\:(\s+[А-Я][а-я]+){1,3}\,\s+(?([3])3[01]|[0-2]?[0-9])\.(?([1])1[0-2]|0[0-9])\.[1-2][0-9][0-9][0-9]";
            char[] separator = new char[] { ' ', ',' };
            string bankTemp = "";
            string clientTemp;
            string line;
            string[] readWord;
            List<Client> clientList = new List<Client>();         

            try
            {
                System.IO.StreamReader file;
                file = new System.IO.StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt"), Encoding.GetEncoding("utf-8"));//1251));

                while ((line = file.ReadLine()) != null)
                {
                    // Проверка на соответствие шаблону банка.
                    if (Regex.IsMatch(line, bankPattern))
                    {
                        readWord = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        bankTemp = readWord[1];
                    }
                    else if (bankTemp == "")
                    {
                        continue;
                    }
                    // Проверка на соответствие шаблону клиента.
                    else if (Regex.IsMatch(line, clientPattern))
                    {
                        readWord = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        clientTemp = "";

                        for (int i = 1; i < readWord.Length - 1; i++)
                        {
                            clientTemp += readWord[i] + " ";
                        }
                        clientTemp = clientTemp.Trim();
                        clientList.Add(new Client() { Fio = clientTemp, Birthday = Convert.ToDateTime(readWord[readWord.Length - 1]), BankName = bankTemp });
                    }
                }
                file.Close();
            }
            catch
            {
                Console.WriteLine("Ошибка заполнения списка клиентов.");
                return null;
            }

            return clientList;
        }

        /// <summary>
        /// Сериализация результата.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool SerializeResult(List<Client> query)
        {
            try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Client>));

                    using (var stream = new FileStream("Result.xml", FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        serializer.Serialize(stream, query);
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
        }

        /// <summary>
        /// Метод изменеия файла input.txt.
        /// </summary>
        /// <param name="clientList"></param>
        internal void PutInfoToFile(List<Client> clientList)
        {
            StreamWriter w = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt"), false, Encoding.GetEncoding("utf-8"));//f.CreateText();

            for (int i = 0; i < clientList.Count; i++)
            {
                if (i == 0 || (clientList[i - 1].BankName != clientList[i].BankName))
                {
                    if (i != 0)
                    {
                        w.WriteLine();
                    }
                    w.WriteLine("Банк: {0}", clientList[i].BankName);
                }

                w.WriteLine("Клиент: {0}, {1}", clientList[i].Fio, String.Format("{0}.{1}.{2}", clientList[i].Birthday.Day, clientList[i].Birthday.Month, clientList[i].Birthday.Year));                
            }
            
            w.Close();
        }
    }
}
