using ArmsServices.DataServices;
using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class BranchModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<BranchModel>(Json);
        }
        public BranchModel()
        {
            Address = new AddressModel();
            UserInfo = new SharedModels.UserInfoModel();
            Place = new PlaceModel();
            Contacts = new();
        }
        private string _branchName;
       
        [Required]
        public string BranchAbbrev { get; set; }        
        private PlaceModel _place;
        public int? BranchID { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string BranchName
        {
            get { return _branchName; }
            set { _branchName = value; Address.AddresseeName = value; }
        }
        [Required]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Gst number must have 15 characters")]
        public string GstNo { get; set; }
        [Required]
        public int? PlaceID { get; set; }
        public int? AddressID { get; set; }
        public bool Operate { get; set; } = true;
        public int? StateOfficeID { get; set; } = 0;
        public bool IsStateOffice { get; set; }
        public string SoDisplayText {
            get
            { return  IsStateOffice ? "SO" : (StateOfficeID??0) == 0?"HO":"Branch"; }
        }
        public bool Active { get; set; } = true;
        [ValidateComplexType]
        public virtual AddressModel Address { get; set; }
        public PlaceModel Place
        {
            get { return _place; }
            set { _place = value; Address.Place = value?.PlaceName; PlaceID = value?.PlaceID; }
        }
        public StateModel State { get; set; }
        public DistrictModel District { get; set; }
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
        public List<ContactModel> Contacts { get; set; }
    }
}