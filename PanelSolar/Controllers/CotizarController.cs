using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using PanelSolar.Helpers;
using PanelSolar.Models;

namespace PanelSolar.Controllers
{
    public class CotizarController : Controller
    {
        public IConfiguration Configuration { get; set; }
        private MailService MailService;
        public CotizarController(MailService MailService, IConfiguration configuration)
        {
            this.MailService = MailService;
            this.Configuration = configuration;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View();
        }

        [ValidateReCaptcha]
        [HttpPost]
        public IActionResult Index(CotizarViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            string body = string.Empty;
            using (StreamReader reader = new StreamReader("./wwwroot/Template/EmailTemplate.html"))
            {
                body = reader.ReadToEnd();
            }

            //Cálculo
            string valConsumoEstimadokWh = "5.629 kWh";
            string valCantidadPaneles = "12 Paneles Clásico";
            string valCapacidadkWh = "5.64 Capacidad total";
            string valAlmacenBateria = "6 kWh Almacenamiento con baterías";
            string valMontoAhorro = "$911.348 Ahorros y beneficios estimados del primer año";
            string valPrecioEstimado = "$11.890.000";


            //Tus Datos
            body = body.Replace("{valNombre}", model.TxtNombre);
            body = body.Replace("{valApellido}", model.TxtApellido);
            body = body.Replace("{valEmail}", model.TxtEmail);
            body = body.Replace("{valNumeroContacto}", model.TxtNumerocomtacto);
            body = body.Replace("{valPrefiereContacto}", model.SelContacto);
            body = body.Replace("{valPorqueCotiza}", model.TxtPorquecotiza);

            //Tu Vivienda
            body = body.Replace("{valTipoVivienda}", model.SelTipoVivienda);
            body = body.Replace("{valDirección}", model.TxtDireccion);
            body = body.Replace("{valDireccionOpcional}", model.TxtDetalleDireccion);
            body = body.Replace("{valComoestuTecho}", model.SelTecho);
            body = body.Replace("{valDequeMaterialeselTecho}", model.TxtMaterialtecho);
            body = body.Replace("{valGastoLuz}", model.TxtGastoluz);
            body = body.Replace("{valMesLuz}", model.TxtMesluz);
            body = body.Replace("{valMasElectricidad}", model.SelConsumoluz);

            //Tu Presupuesto
            body = body.Replace("{valAgregarBateria}", model.SelAgregarbateria);
            body = body.Replace("{valAgregarCargador}", model.SelAgregarcargadorvehiculo);
            body = body.Replace("{valPorqueEnergiaSolar}", model.SelAinteresenergiasolar);

            //Tu Resultado
            body = body.Replace("{valConsumoEstimadokWh}", valConsumoEstimadokWh);
            body = body.Replace("{valCantidadPaneles}", valCantidadPaneles);
            body = body.Replace("{valCapacidadkWh}", valCapacidadkWh);
            body = body.Replace("{valAlmacenBateria}", valAlmacenBateria);
            body = body.Replace("{valMontoAhorro}", valMontoAhorro);
            body = body.Replace("{valPrecioEstimado}", valPrecioEstimado);

            String Ok = "";
            try
            {
                this.MailService.SendEmailOutlook(model.TxtNombre + ' ' + model.TxtApellido,
                                                  model.TxtEmail != null ? model.TxtEmail : "",
                                                  "Propuesta Instalación de Panel Solar",
                                                  body,
                                                  "");
                Ok = "Ok";
                TempData["Success"] = "Estimado: " + model.TxtNombre + ", hemos recibido tu email y pronto nos contactaremos contigo. Atte. Equipo https://PremiumSolar.cl";
            }
            catch (ApplicationException x)
            {
                Ok = "NoOk";
                TempData["Success"] = x.Message;

            }
            catch (Exception e)
            {
                Ok = "NoOk";
                TempData["Success"] = e.Message;
            }


            if (Ok == "Ok")
            {
                //Propuesta - Final
                TempData["nombreApellido"] = model.TxtNombre + ' ' + model.TxtApellido;
                TempData["ConsumoEstimadokWh"] = valConsumoEstimadokWh;
                TempData["CantidadPaneles"] = valCantidadPaneles;
                TempData["CapacidadkWh"] = valCapacidadkWh;
                TempData["AlmacenBateria"] = valAlmacenBateria;
                TempData["MontoAhorro"] = valMontoAhorro;
                TempData["PrecioEstimado"] = valPrecioEstimado;

                return Redirect("../Propuesta");
            }
            else
            {
                TempData["Error"] = "Error 550";
                return Redirect("../Error550");
            }


        }


    }
}
