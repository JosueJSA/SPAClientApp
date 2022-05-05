﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SPAClientApp.PedidosClientesService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EPedidoCliente", Namespace="http://schemas.datacontract.org/2004/07/Services.Model")]
    [System.SerializableAttribute()]
    public partial class EPedidoCliente : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double CantidadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double CostoTotalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime EntregaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> NumeroProductosField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime SolicitudField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TipoPedidoField;
        
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
        public double CostoTotal {
            get {
                return this.CostoTotalField;
            }
            set {
                if ((this.CostoTotalField.Equals(value) != true)) {
                    this.CostoTotalField = value;
                    this.RaisePropertyChanged("CostoTotal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Entrega {
            get {
                return this.EntregaField;
            }
            set {
                if ((this.EntregaField.Equals(value) != true)) {
                    this.EntregaField = value;
                    this.RaisePropertyChanged("Entrega");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> NumeroProductos {
            get {
                return this.NumeroProductosField;
            }
            set {
                if ((this.NumeroProductosField.Equals(value) != true)) {
                    this.NumeroProductosField = value;
                    this.RaisePropertyChanged("NumeroProductos");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Solicitud {
            get {
                return this.SolicitudField;
            }
            set {
                if ((this.SolicitudField.Equals(value) != true)) {
                    this.SolicitudField = value;
                    this.RaisePropertyChanged("Solicitud");
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
        public string TipoPedido {
            get {
                return this.TipoPedidoField;
            }
            set {
                if ((object.ReferenceEquals(this.TipoPedidoField, value) != true)) {
                    this.TipoPedidoField = value;
                    this.RaisePropertyChanged("TipoPedido");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="EProductoComprado", Namespace="http://schemas.datacontract.org/2004/07/Services.Model")]
    [System.SerializableAttribute()]
    public partial class EProductoComprado : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CantidadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoPedidoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoProductoVentaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> CodigoRecetaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescripcionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FotoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double PrecioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double PrecioCompraField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double PrecioVentaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RestriccionesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int stockField;
        
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
        public int Cantidad {
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
        public int CodigoPedido {
            get {
                return this.CodigoPedidoField;
            }
            set {
                if ((this.CodigoPedidoField.Equals(value) != true)) {
                    this.CodigoPedidoField = value;
                    this.RaisePropertyChanged("CodigoPedido");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CodigoProductoVenta {
            get {
                return this.CodigoProductoVentaField;
            }
            set {
                if ((this.CodigoProductoVentaField.Equals(value) != true)) {
                    this.CodigoProductoVentaField = value;
                    this.RaisePropertyChanged("CodigoProductoVenta");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> CodigoReceta {
            get {
                return this.CodigoRecetaField;
            }
            set {
                if ((this.CodigoRecetaField.Equals(value) != true)) {
                    this.CodigoRecetaField = value;
                    this.RaisePropertyChanged("CodigoReceta");
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
        public string Foto {
            get {
                return this.FotoField;
            }
            set {
                if ((object.ReferenceEquals(this.FotoField, value) != true)) {
                    this.FotoField = value;
                    this.RaisePropertyChanged("Foto");
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
        public double Precio {
            get {
                return this.PrecioField;
            }
            set {
                if ((this.PrecioField.Equals(value) != true)) {
                    this.PrecioField = value;
                    this.RaisePropertyChanged("Precio");
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
        public double PrecioVenta {
            get {
                return this.PrecioVentaField;
            }
            set {
                if ((this.PrecioVentaField.Equals(value) != true)) {
                    this.PrecioVentaField = value;
                    this.RaisePropertyChanged("PrecioVenta");
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
        public int stock {
            get {
                return this.stockField;
            }
            set {
                if ((this.stockField.Equals(value) != true)) {
                    this.stockField = value;
                    this.RaisePropertyChanged("stock");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ECliente", Namespace="http://schemas.datacontract.org/2004/07/Services.Model")]
    [System.SerializableAttribute()]
    public partial class ECliente : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApellidoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CalleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CiudadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoPostalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ColoniaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int EdadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdDireccionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime NacimientoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int NumeroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TelefonoField;
        
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
        public string Apellido {
            get {
                return this.ApellidoField;
            }
            set {
                if ((object.ReferenceEquals(this.ApellidoField, value) != true)) {
                    this.ApellidoField = value;
                    this.RaisePropertyChanged("Apellido");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Calle {
            get {
                return this.CalleField;
            }
            set {
                if ((object.ReferenceEquals(this.CalleField, value) != true)) {
                    this.CalleField = value;
                    this.RaisePropertyChanged("Calle");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Ciudad {
            get {
                return this.CiudadField;
            }
            set {
                if ((object.ReferenceEquals(this.CiudadField, value) != true)) {
                    this.CiudadField = value;
                    this.RaisePropertyChanged("Ciudad");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CodigoPostal {
            get {
                return this.CodigoPostalField;
            }
            set {
                if ((this.CodigoPostalField.Equals(value) != true)) {
                    this.CodigoPostalField = value;
                    this.RaisePropertyChanged("CodigoPostal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Colonia {
            get {
                return this.ColoniaField;
            }
            set {
                if ((object.ReferenceEquals(this.ColoniaField, value) != true)) {
                    this.ColoniaField = value;
                    this.RaisePropertyChanged("Colonia");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Edad {
            get {
                return this.EdadField;
            }
            set {
                if ((this.EdadField.Equals(value) != true)) {
                    this.EdadField = value;
                    this.RaisePropertyChanged("Edad");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IdDireccion {
            get {
                return this.IdDireccionField;
            }
            set {
                if ((this.IdDireccionField.Equals(value) != true)) {
                    this.IdDireccionField = value;
                    this.RaisePropertyChanged("IdDireccion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Nacimiento {
            get {
                return this.NacimientoField;
            }
            set {
                if ((this.NacimientoField.Equals(value) != true)) {
                    this.NacimientoField = value;
                    this.RaisePropertyChanged("Nacimiento");
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
        public int Numero {
            get {
                return this.NumeroField;
            }
            set {
                if ((this.NumeroField.Equals(value) != true)) {
                    this.NumeroField = value;
                    this.RaisePropertyChanged("Numero");
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
        public string Telefono {
            get {
                return this.TelefonoField;
            }
            set {
                if ((object.ReferenceEquals(this.TelefonoField, value) != true)) {
                    this.TelefonoField = value;
                    this.RaisePropertyChanged("Telefono");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="EProducto", Namespace="http://schemas.datacontract.org/2004/07/Services.Model")]
    [System.SerializableAttribute()]
    public partial class EProducto : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CantidadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> CodigoRecetaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescripcionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FotoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double PrecioCompraField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double PrecioVentaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime RegistroField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RestriccionesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
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
        public int Cantidad {
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
        public System.Nullable<int> CodigoReceta {
            get {
                return this.CodigoRecetaField;
            }
            set {
                if ((this.CodigoRecetaField.Equals(value) != true)) {
                    this.CodigoRecetaField = value;
                    this.RaisePropertyChanged("CodigoReceta");
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
        public string Foto {
            get {
                return this.FotoField;
            }
            set {
                if ((object.ReferenceEquals(this.FotoField, value) != true)) {
                    this.FotoField = value;
                    this.RaisePropertyChanged("Foto");
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
        public double PrecioVenta {
            get {
                return this.PrecioVentaField;
            }
            set {
                if ((this.PrecioVentaField.Equals(value) != true)) {
                    this.PrecioVentaField = value;
                    this.RaisePropertyChanged("PrecioVenta");
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
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PedidosClientesService.IPedidosClientesService")]
    public interface IPedidosClientesService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetPedidosClientesList", ReplyAction="http://tempuri.org/IPedidosClientesService/GetPedidosClientesListResponse")]
        SPAClientApp.PedidosClientesService.EPedidoCliente[] GetPedidosClientesList(string criterio, System.DateTime fecha);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetPedidosClientesList", ReplyAction="http://tempuri.org/IPedidosClientesService/GetPedidosClientesListResponse")]
        System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EPedidoCliente[]> GetPedidosClientesListAsync(string criterio, System.DateTime fecha);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetCommonPedidosList", ReplyAction="http://tempuri.org/IPedidosClientesService/GetCommonPedidosListResponse")]
        SPAClientApp.PedidosClientesService.EPedidoCliente[] GetCommonPedidosList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetCommonPedidosList", ReplyAction="http://tempuri.org/IPedidosClientesService/GetCommonPedidosListResponse")]
        System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EPedidoCliente[]> GetCommonPedidosListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetProductosComprados", ReplyAction="http://tempuri.org/IPedidosClientesService/GetProductosCompradosResponse")]
        SPAClientApp.PedidosClientesService.EProductoComprado[] GetProductosComprados(int clavePedido);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetProductosComprados", ReplyAction="http://tempuri.org/IPedidosClientesService/GetProductosCompradosResponse")]
        System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EProductoComprado[]> GetProductosCompradosAsync(int clavePedido);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetClienteInfo", ReplyAction="http://tempuri.org/IPedidosClientesService/GetClienteInfoResponse")]
        SPAClientApp.PedidosClientesService.ECliente GetClienteInfo(int IDCliente, int IDDirección);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetClienteInfo", ReplyAction="http://tempuri.org/IPedidosClientesService/GetClienteInfoResponse")]
        System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.ECliente> GetClienteInfoAsync(int IDCliente, int IDDirección);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetProductosList", ReplyAction="http://tempuri.org/IPedidosClientesService/GetProductosListResponse")]
        SPAClientApp.PedidosClientesService.EProducto[] GetProductosList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPedidosClientesService/GetProductosList", ReplyAction="http://tempuri.org/IPedidosClientesService/GetProductosListResponse")]
        System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EProducto[]> GetProductosListAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPedidosClientesServiceChannel : SPAClientApp.PedidosClientesService.IPedidosClientesService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PedidosClientesServiceClient : System.ServiceModel.ClientBase<SPAClientApp.PedidosClientesService.IPedidosClientesService>, SPAClientApp.PedidosClientesService.IPedidosClientesService {
        
        public PedidosClientesServiceClient() {
        }
        
        public PedidosClientesServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PedidosClientesServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PedidosClientesServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PedidosClientesServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SPAClientApp.PedidosClientesService.EPedidoCliente[] GetPedidosClientesList(string criterio, System.DateTime fecha) {
            return base.Channel.GetPedidosClientesList(criterio, fecha);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EPedidoCliente[]> GetPedidosClientesListAsync(string criterio, System.DateTime fecha) {
            return base.Channel.GetPedidosClientesListAsync(criterio, fecha);
        }
        
        public SPAClientApp.PedidosClientesService.EPedidoCliente[] GetCommonPedidosList() {
            return base.Channel.GetCommonPedidosList();
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EPedidoCliente[]> GetCommonPedidosListAsync() {
            return base.Channel.GetCommonPedidosListAsync();
        }
        
        public SPAClientApp.PedidosClientesService.EProductoComprado[] GetProductosComprados(int clavePedido) {
            return base.Channel.GetProductosComprados(clavePedido);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EProductoComprado[]> GetProductosCompradosAsync(int clavePedido) {
            return base.Channel.GetProductosCompradosAsync(clavePedido);
        }
        
        public SPAClientApp.PedidosClientesService.ECliente GetClienteInfo(int IDCliente, int IDDirección) {
            return base.Channel.GetClienteInfo(IDCliente, IDDirección);
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.ECliente> GetClienteInfoAsync(int IDCliente, int IDDirección) {
            return base.Channel.GetClienteInfoAsync(IDCliente, IDDirección);
        }
        
        public SPAClientApp.PedidosClientesService.EProducto[] GetProductosList() {
            return base.Channel.GetProductosList();
        }
        
        public System.Threading.Tasks.Task<SPAClientApp.PedidosClientesService.EProducto[]> GetProductosListAsync() {
            return base.Channel.GetProductosListAsync();
        }
    }
}
