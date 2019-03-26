using System;

namespace $safeprojectname$.Util
{
    public static class Static
    {
        public static string GerarSenha()
        {
            var tamanhoDaSenha = 10;
            var caracteresPermitidos = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            var chars = new char[tamanhoDaSenha];
            var rd = new Random();
            for (int i = 0; i < tamanhoDaSenha; i++)
            {
                chars[i] = caracteresPermitidos[rd.Next(0, caracteresPermitidos.Length)];
            }
            return new string(chars);
        }
    }
}