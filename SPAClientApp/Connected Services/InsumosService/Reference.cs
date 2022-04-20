﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SPAClientApp.InsumosService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EInsumo", Namespace="http://schemas.datacontract.org/2004/07/Services.Model")]
    [System.SerializableAttribute()]
    public partial class EInsumo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double CantidadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescripcionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double PrecioCompraField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProveedorDeInsumoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime RegistroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RestriccionesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UnidadMedidaField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Cantidad {
            get {
                return this.CantidadField;
            }
            set {
                if ((this.CantidadField.Equals(value) != true)) {
                    this.CantidadField = value;
                    this.RaisePropertyChanged("Cantidad");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((this.CodigoField.Equals(value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Descripcion {
            get {
                return this.DescripcionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescripcionField, value) != true)) {
                    this.DescripcionField = value;
                    this.RaisePropertyChanged("Descripcion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreField, value) != true)) {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double PrecioCompra {
            get {
                return this.PrecioCompraField;
            }
            set {
                if ((this.PrecioCompraField.Equals(value) != true)) {
                    this.PrecioCompraField = value;
                    this.RaisePropertyChanged("PrecioCompra");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProveedorDeInsumo {
            get {
                return this.ProveedorDeInsumoField;
            }
            set {
                if ((object.ReferenceEquals(this.ProveedorDeInsumoField, value) != true)) {
                    this.ProveedorDeInsumoField = value;
                    this.RaisePropertyChanged("ProveedorDeInsumo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Registro {
            get {
                return this.RegistroField;
            }
            set {
                if ((this.RegistroField.Equals(value) != true)) {
                    this.RegistroField = value;
                    this.RaisePropertyChanged("Registro");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Restricciones {
            get {
                return this.RestriccionesField;
            }
            set {
                if ((object.ReferenceEquals(this.RestriccionesField, value) != true)) {
                    this.RestriccionesField = value;
                    this.RaisePropertyChanged("Restricciones");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UnidadMedida {
            get {
                return this.UnidadMedidaField;
            }
            set {
                if ((object.ReferenceEquals(this.UnidadMedidaField, value) != true)) {
                    this.UnidadMedidaField = value;
                    this.RaisePropertyChanged("UnidadMedida");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AnswerMessage", Namespace="http://schemas.datacontract.org/2004/07/Services.Model")]
    [System.SerializableAttribute()]
    public partial class AnswerMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int KeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Key {
            get {
                return this.KeyField;
            }
            set {
                if ((this.KeyField.Equals(value) != true)) {
                    this.KeyField = value;
                    this.RaisePropertyChanged("Key");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="InsumosService.IInsumosService")]
    public interface IInsumosService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/GetInsumosList", ReplyAction="http://tempuri.org/IInsumosService/GetInsumosListResponse")]
        SPAClientApp.InsumosService.EInsumo[] GetInsumosList(string criterio, string valor, System.DateTime fecha, string status);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/GetInsumosList", ReplyAction="http://tempuri.org/IInsumosService/GetInsumosListResponse")]
        System.Threading.Tasks.Task<SPAClientApp.InsumosService.EInsumo[]> GetInsumosListAsync(string criterio, string valor, System.DateTime fecha, string status);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/AddInsumo", ReplyAction="http://tempuri.org/IInsumosService/AddInsumoResponse")]
        SPAClientApp.InsumosService.AnswerMessage AddInsumo(SPAClientApp.InsumosService.EInsumo insumo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/AddInsumo", ReplyAction="http://tempuri.org/IInsumosService/AddInsumoResponse")]
        System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> AddInsumoAsync(SPAClientApp.InsumosService.EInsumo insumo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/UpdateInsumo", ReplyAction="http://tempuri.org/IInsumosService/UpdateInsumoResponse")]
        SPAClientApp.InsumosService.AnswerMessage UpdateInsumo(int oldInsumoID, SPAClientApp.InsumosService.EInsumo newInsumo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/UpdateInsumo", ReplyAction="http://tempuri.org/IInsumosService/UpdateInsumoResponse")]
        System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> UpdateInsumoAsync(int oldInsumoID, SPAClientApp.InsumosService.EInsumo newInsumo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/ChangeInsumoStatus", ReplyAction="http://tempuri.org/IInsumosService/ChangeInsumoStatusResponse")]
        SPAClientApp.InsumosService.AnswerMessage ChangeInsumoStatus(int insumoID, string status);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/ChangeInsumoStatus", ReplyAction="http://tempuri.org/IInsumosService/ChangeInsumoStatusResponse")]
        System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> ChangeInsumoStatusAsync(int insumoID, string status);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/ActualizarCantidades", ReplyAction="http://tempuri.org/IInsumosService/ActualizarCantidadesResponse")]
        SPAClientApp.InsumosService.AnswerMessage ActualizarCantidades(SPAClientApp.InsumosService.EInsumo[] nuevasCantidades);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/ActualizarCantidades", ReplyAction="http://tempuri.org/IInsumosService/ActualizarCantidadesResponse")]
        System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> ActualizarCantidadesAsync(SPAClientApp.InsumosService.EInsumo[] nuevasCantidades);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/IsDuplicated", ReplyAction="http://tempuri.org/IInsumosService/IsDuplicatedResponse")]
        bool IsDuplicated(string nombreActual, string nombreABuscar);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/IsDuplicated", ReplyAction="http://tempuri.org/IInsumosService/IsDuplicatedResponse")]
        System.Threading.Tasks.Task<bool> IsDuplicatedAsync(string nombreActual, string nombreABuscar);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/IsUsedInReceta", ReplyAction="http://tempuri.org/IInsumosService/IsUsedInRecetaResponse")]
        bool IsUsedInReceta(int codigoInsumo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IInsumosService/IsUsedInReceta", ReplyAction="http://tempuri.org/IInsumosService/IsUsedInRecetaResponse")]
        System.Threading.Tasks.Task<bool> IsUsedInRecetaAsync(int codigoInsumo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IInsumosServiceChannel : SPAClientApp.InsumosService.IInsumosService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InsumosServiceClient : System.ServiceModel.ClientBase<SPAClientApp.InsumosService.IInsumosService>, SPAClientApp.InsumosService.IInsumosService {
        
        public InsumosServiceClient() {
        }
        
        public InsumosServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InsumosServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InsumosServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InsumosServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SPAClientApp.InsumosService.EInsumo[] GetInsumosList(string criterio, string valor, System.DateTime fecha, string status) {
            return base.Channel.GetInsumosList(criterio, valor, fecha, status);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.InsumosService.EInsumo[]> GetInsumosListAsync(string criterio, string valor, System.DateTime fecha, string status) {
            return base.Channel.GetInsumosListAsync(criterio, valor, fecha, status);
        }
        
        public SPAClientApp.InsumosService.AnswerMessage AddInsumo(SPAClientApp.InsumosService.EInsumo insumo) {
            return base.Channel.AddInsumo(insumo);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> AddInsumoAsync(SPAClientApp.InsumosService.EInsumo insumo) {
            return base.Channel.AddInsumoAsync(insumo);
        }
        
        public SPAClientApp.InsumosService.AnswerMessage UpdateInsumo(int oldInsumoID, SPAClientApp.InsumosService.EInsumo newInsumo) {
            return base.Channel.UpdateInsumo(oldInsumoID, newInsumo);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> UpdateInsumoAsync(int oldInsumoID, SPAClientApp.InsumosService.EInsumo newInsumo) {
            return base.Channel.UpdateInsumoAsync(oldInsumoID, newInsumo);
        }
        
        public SPAClientApp.InsumosService.AnswerMessage ChangeInsumoStatus(int insumoID, string status) {
            return base.Channel.ChangeInsumoStatus(insumoID, status);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> ChangeInsumoStatusAsync(int insumoID, string status) {
            return base.Channel.ChangeInsumoStatusAsync(insumoID, status);
        }
        
        public SPAClientApp.InsumosService.AnswerMessage ActualizarCantidades(SPAClientApp.InsumosService.EInsumo[] nuevasCantidades) {
            return base.Channel.ActualizarCantidades(nuevasCantidades);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.InsumosService.AnswerMessage> ActualizarCantidadesAsync(SPAClientApp.InsumosService.EInsumo[] nuevasCantidades) {
            return base.Channel.ActualizarCantidadesAsync(nuevasCantidades);
        }
        
        public bool IsDuplicated(string nombreActual, string nombreABuscar) {
            return base.Channel.IsDuplicated(nombreActual, nombreABuscar);
        }
        
        public System.Threading.Tasks.Task<bool> IsDuplicatedAsync(string nombreActual, string nombreABuscar) {
            return base.Channel.IsDuplicatedAsync(nombreActual, nombreABuscar);
        }
        
        public bool IsUsedInReceta(int codigoInsumo) {
            return base.Channel.IsUsedInReceta(codigoInsumo);
        }
        
        public System.Threading.Tasks.Task<bool> IsUsedInRecetaAsync(int codigoInsumo) {
            return base.Channel.IsUsedInRecetaAsync(codigoInsumo);
        }
    }
}