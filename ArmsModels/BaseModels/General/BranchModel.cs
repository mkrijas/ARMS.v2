using ArmsServices.DataServices;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class BranchModel
    {
        public BranchModel()
        {
            Address = new AddressModel();
            UserInfo = new SharedModels.UserInfoModel();
            Place = new PlaceModel();
            Contacts = new();
        }
        private string _branchName;
        public string BranchCode { get; set; }
        //[Required]
        //[StringLength(3, MinimumLength = 3, ErrorMessage = "Abbrev number must have 3 characters")]
        public string BranchAbbrev { get; set; }
        public ChartOfAccountModel Coa { get; set; }
        private PlaceModel _place;
        public int? BranchID { get; set; }
        [Required]
        [StringLength(256,MinimumLength =3)]
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
        public int? UpwardBranchID { get; set; } = 0;
        public bool Active { get; set; } = true;
        [ValidateComplexType]
        public virtual AddressModel Address { get; set; }        
        public PlaceModel Place { 
            get { return _place; } 
            set { _place = value; Address.Place = value?.PlaceName;  PlaceID = value?.PlaceID; } 
        }
        public StateModel State { get; set; }
        public DistrictModel District { get; set; }
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
        public List<ContactModel> Contacts { get; set; }
    }

    //public class GstCodeValidator : AbstractValidator<BranchModel>
    //{
    //    public GstCodeValidator(IBranchService _IBranchService)
    //    {
    //        RuleFor(P => P.GstNo)
    //           .NotEmpty().WithMessage("GstNo cannot be empty !")
    //           .Must((branchModel, GstNo) =>
    //                   ValidateGstNo(_IBranchService, branchModel.GstNo, branchModel.PlaceID.Value.ToString()))
    //           .WithMessage("GstNo is not matching with State GstCode !");
    //    }

    //    private bool ValidateGstNo(IBranchService _IBranchService, string GstNo, string PlaceID)
    //    {
    //        if (GstNo != null && PlaceID != null && Convert.ToInt32(PlaceID)!= 0)
    //        {
    //            BranchModel GstCodeModel = _IBranchService.ValidateGstNo(Convert.ToInt32(PlaceID));
    //            var gststatecode = GstCodeModel.GstCode;
    //            var gstcode = GstNo.Substring(0, 2);
    //            if (gststatecode.Value.ToString() == gstcode)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}
}
