using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          string hey =  AesEncryption.EncryptString(richTextBox1.Text, "A3F25678901D2345A8C9E6F123456789");

            richTextBox2.Text = hey;
        }


        public static string DecryptString(string cipherText, string keyString)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - 16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            byte[] key = Encoding.UTF8.GetBytes(keyString);
            using (Aes aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    using (var msDecrypt = new MemoryStream(cipher))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        public static class AesEncryption
        {
            public static string EncryptString(string plainText, string keyString)
            {
                byte[] key = Encoding.UTF8.GetBytes(keyString);
                using (Aes aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }

                            var iv = aesAlg.IV;
                            var encryptedContent = msEncrypt.ToArray();
                            var result = new byte[iv.Length + encryptedContent.Length];
                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nonono = DecryptString(richTextBox3.Text, "A3F25678901D2345A8C9E6F123456789");

            richTextBox4.Text = nonono;
        }
    }
}
