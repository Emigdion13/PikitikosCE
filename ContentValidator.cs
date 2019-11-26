using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PikitikosCE
{
    public static class ContentValidator
    {

        //Need to Update it to extension



        /// <summary>
        /// opciones disponibles: nulo, numerico, decimal, cedula, ,social_id, telefono, 4digitos, 2digitos, email
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="opcion"></param>
        /// <returns></returns>
        public static string valida_campo(object tb, string opcion)
        {
            string mensaje_error = "";

            if (opcion.Contains("nulo"))
            {
                if (string.IsNullOrEmpty((tb as Control).Text.Trim()))
                {
                    mensaje_error += "The content cannot be empty";
                }

            }

            if (opcion.Contains("numerico"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";

                int resultado;
                if (!int.TryParse((tb as Control).Text.Trim(), out resultado))
                {
                    mensaje_error += "The content must be numberic";
                }

            }

            if (opcion.ToLower().Contains("decimal"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";

                double resultado;
                if (!double.TryParse((tb as Control).Text.Trim(), out resultado))
                {
                    mensaje_error += "The content is not a valid decimal number, Ex: 19.00, 19, 1.93 are valid examples";
                }
            }

            if (opcion.Contains("cedula"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";
                if (!Regex.IsMatch((tb as Control).Text.Trim(), @"^\d{3}-?\d{7}-?\d$", RegexOptions.Compiled))
                {
                    mensaje_error += "La cedula es invalida";
                }
            }

            if (opcion.Contains("social_id"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";
                if (!Regex.IsMatch((tb as Control).Text.Trim(), @"^[\d]{3}\d{2}\d{4}$", RegexOptions.Compiled))
                {
                    mensaje_error += "The social security id seems to not be valid, use only numbers without marks";
                }
            }

            if (opcion.Contains("telefono"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";
                if (!Regex.IsMatch((tb as Control).Text.Trim(), @"^[\d]{10}$", RegexOptions.Compiled))
                {
                    mensaje_error += "Please specify a valid phone number format, use only numbers without any marks";
                }
            }

            if (opcion.Contains("4digitos"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";
                if ((tb as Control).Text.Trim().Count() < 4)
                {
                    mensaje_error += "Please specify a value greater than 4 digits";
                }
            }

            if (opcion.Contains("2digitos"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";
                if ((tb as Control).Text.Trim().Count() < 2)
                {
                    mensaje_error += "Please specify a value greater than 2 digits";
                }
            }

            if (opcion.Contains("6digitos"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";
                if ((tb as Control).Text.Trim().Count() < 6)
                {
                    mensaje_error += "Please specify a value greater than 6 digits";
                }
            }

            if (opcion.Contains("email"))
            {
                if (!string.IsNullOrEmpty(mensaje_error)) mensaje_error += ", ";
                try
                {
                    string email = (tb as Control).Text.Trim();
                    var addr = new System.Net.Mail.MailAddress(email);
                }
                catch
                {
                    mensaje_error += "Please specify a valid email address";
                }
            }

            return mensaje_error;
        }
    }
}
