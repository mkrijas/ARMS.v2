using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using TableDependency.SqlClient.Base;

namespace Core.BaseModels.Finance.Transactions
{
    public class FastTagTollModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<FastTagTollModel>(Json);
        }
        public int? FastTagUploadID { get; set; }
        public string ProcessDocumentNumber { get; set; } = "New";
        public string ReverseDocumentNumber { get; set; }
        [Required]
        public string PaymentMode { get; set; }
        [Required]
        public string PaymentArdCode { get; set; }
        [Required]
        public int? PaymentCoaID { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public string PaymentTool { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public decimal? BankCharges { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<FastTagModel> FastTagModelList { get; set; } = new();
    }

    public class FastTagProcessModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<FastTagProcessModel>(Json);
        }
        public int? FastTagProcessID { get; set; }
        public int? FastTagProcessUploadID { get; set; }
        public string ProcessDocumentNumber { get; set; } = "New";
        [Required]
        public DateTime? DocumentDate { get; set; } = DateTime.Today;
        [Required]
        public string Narration { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<FastTagModel> FastTagModelList { get; set; } = new();
    }

    public class FastTagModel
    {
        public int? FastTagTollID { get; set; }
        public int? FastTagProcessID { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public DateTime? ProcessedDateTime { get; set; }
        public int? BranchID { get; set; }
        public int? TruckID { get; set; }
        public long? TripID { get; set; }
        //public long? TripNumber { get; set; }
        public virtual string TripNumberDisplay { get; set; }
        public string NumberPlate { get; set; }
        public string PlazaCode { get; set; }
        public string Description { get; set; }
        public string TransactionID { get; set; }
        public bool Reimbursable { get; set; }
        public decimal DebitAmount { get; set; }
        public virtual string Destination { get; set; }
        public virtual Boolean IsProcessed { get; set; }
        public virtual Boolean IsChecked { get; set; }
        public virtual string Activity { get; set; }
        public virtual string BranchName { get; set; }
        //public virtual string TripPrefix { get; set; }
        //public virtual string TripNumberDisplay
        //{
        //    get
        //    {
        //        if (TripPrefix == null || TripNumber == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return TripPrefix + TripNumber.ToString().PadLeft(4, '0');
        //        }
        //    }
        //}
        public virtual decimal CreditAmount { get; set; }
        public virtual byte? RecordStatus { get; set; }
    }

    public class FastTagList
    {
        public DateTime? TransactionDateTime { get; set; }
        public string NumberPlate { get; set; }
    }

    public class FastTagBranchEditModel
    {
        public int? FastTagTollID { get; set; }
        [Required]
        public string TransactionID { get; set; }
        //public string TripNumberDisplay
        //{
        //    get
        //    {
        //        if (TripPrefix == null || TripNumber == null)
        //        {
        //            return "";
        //        }
        //        else
        //        {
        //            return TripPrefix + TripNumber.ToString().PadLeft(4, '0');
        //        }
        //    }
        //    set
        //    {
        //        SetTripPrefixAndTripNumber(value);
        //    }
        //}
        //public string TripPrefix { get; set; }
        //public long? TripNumber { get; set; }
        [Required]
        public string ActivityType { get; set; }
        [Required]
        public string TripNumberDisplay { get; set; }
        [Required]
        public BranchModel Branch { get; set; }
        public UserInfoModel UserInfo { get; set; } = new UserInfoModel();
        //private void SetTripPrefixAndTripNumber(string tripNumberDisplay)
        //{
        //    if (string.IsNullOrEmpty(tripNumberDisplay))
        //    {
        //        TripPrefix = null;
        //        TripNumber = null;
        //        return;
        //    }

        //    // Find the last '/' in the string
        //    int lastSlashIndex = tripNumberDisplay.LastIndexOf('/');
        //    if (lastSlashIndex != -1 && lastSlashIndex < tripNumberDisplay.Length - 1)
        //    {
        //        TripPrefix = tripNumberDisplay.Substring(0, lastSlashIndex + 1);
        //        string tripNumberString = tripNumberDisplay.Substring(lastSlashIndex + 1);

        //        if (long.TryParse(tripNumberString, out long tripNumber))
        //        {
        //            TripNumber = tripNumber;
        //        }
        //        else
        //        {
        //            TripPrefix = null;
        //            TripNumber = null;
        //        }
        //    }
        //    else
        //    {
        //        TripPrefix = null;
        //        TripNumber = null;
        //    }
        //}
    }

    public static class EncryptionHelper
    {
        private static byte[] Key;
        private static byte[] IV;

        static EncryptionHelper()
        {
            // Generate a random key and IV
            GenerateRandomKeyAndIV();
        }

        private static (byte[] key, byte[] iv) GenerateRandomKeyAndIV()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[16]; // 128-bit key for AES-128
                byte[] iv = new byte[16]; // IV length should match block size (128 bits)

                for (int i = 0; i < 16; i++)
                {
                    byte[] randomeKeyByte = new byte[1];
                    byte[] randomIVByte = new byte[1];
                    do
                    {
                        rng.GetBytes(randomeKeyByte);
                        rng.GetBytes(randomIVByte);
                    }
                    while ((randomeKeyByte[0] == '/') || (randomIVByte[0] == '/'));
                    key[i] = randomeKeyByte[0];
                    iv[i] = randomIVByte[0];
                }
                return (key, iv);
            }
        }

        public static string Encrypt(string text)
        {
            try
            {
                using var aes = Aes.Create();
                Key = aes.Key;
                IV = aes.IV;

                //var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                var encryptor = aes.CreateEncryptor(Key, IV);
                var encryptedBytes = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(text), 0, text.Length);
                //return Convert.ToBase64String(encryptedBytes);
                var base64 = Convert.ToBase64String(encryptedBytes);
                return base64.Replace('+', '-').Replace('/', '_');
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                using var aes = Aes.Create();

                //var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var decryptor = aes.CreateDecryptor(Key, IV);
                cipherText = cipherText.Replace('-', '+').Replace('_', '/');
                var cipherBytes = Convert.FromBase64String(cipherText);
                var decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}