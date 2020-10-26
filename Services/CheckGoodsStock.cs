using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Finance.Services
{
    public class CheckGoodsStock
    {
        public static void AppStart()
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("andriiboiko82@gmail.com", "Master-1Finance");
            // кому отправляем
            MailAddress to = new MailAddress("vinil2001@ukr.net");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Тест";
            // текст письма
            m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("andriiboiko82@gmail.com", "VinilNomad82");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}