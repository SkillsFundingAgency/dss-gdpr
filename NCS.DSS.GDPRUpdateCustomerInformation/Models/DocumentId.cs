using DFC.Swagger.Standard.Annotations;
using System.ComponentModel.DataAnnotations;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Models
{
    public class DocumentId
    {
        [Display(Description = "Unique identifier of a document")]
        [Example(Description = "b8592ff8-af97-49ad-9fb2-e5c3c717fd85")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [Display(Description = "Unique identifier of a customer")]
        [Example(Description = "b8592ff8-af97-49ad-9fb2-e5c3c717fd85")]
        [Newtonsoft.Json.JsonProperty(PropertyName = "CustomerId")]
        public Guid? CustomerId { get; set; }
    }
}
