using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ManejadorBD
{
    public class ManejaBD
    {
        private DbProviderFactory _objFactory = null;
        private DbConnection _objConex = null;
        private DbCommand _objCommand = null;
        private DbDataAdapter _objAdapter = null;
        private string _strErrBase = string.Empty;
        private string _Connection = string.Empty;
        private string _StoredProcedure = string.Empty;
        public string Error
        {
            get { return _strErrBase; }
        }
        public string StoredProcedure
        {
            set { this._StoredProcedure = value; }
            get { return _StoredProcedure; }
        }

        public ManejaBD(ConnectionBD _conexionBD, eProveedor _proveedor)
        {
            _Connection = string.Empty;
            try
            {
                _Connection = ConfigurationManager.ConnectionStrings[_conexionBD.ToString()].ConnectionString;
                Constructor(_Connection, _proveedor);
            }
            catch (Exception ex)
            {
                _strErrBase = ex.Message;
            }
        }
        private void Constructor(string _conexionBD, eProveedor _proveedor)
        {
            try
            {
                string _strProv = getProveedor(_proveedor);
                _objFactory = DbProviderFactories.GetFactory(_strProv);
                _objConex = _objFactory.CreateConnection();
                _objCommand = _objFactory.CreateCommand();
                _objAdapter = _objFactory.CreateDataAdapter();
            }
            catch (Exception ex)
            {
                _strErrBase = ex.Message;
            }
        }

        public DataSet EjecutarQuery(string _strSQL)
        {
            DataSet _dtsRes = new DataSet();
            SqlConnection _objSqlCnn;
            SqlDataAdapter _objSqlAdap;
            Exception _ObjEx;
            _objSqlCnn = AbreConex();
            try
            {
                _objSqlAdap = new SqlDataAdapter(_strSQL, _objSqlCnn);
                _objSqlAdap.SelectCommand.CommandTimeout = 0;
                _objSqlAdap.Fill(_dtsRes, "REGISTRO");
                return _dtsRes;
            }
            catch (Exception ex)
            {
                _ObjEx = new Exception("Error en la comunicacion a la base de datos");
                _objSqlCnn.Close();
                throw _ObjEx;
            }
        }
        private SqlConnection AbreConex()
        {
            SqlConnection _result;
            _result = new SqlConnection(_Connection);
            return _result;
        }
        public void AgregaParametro(string nombreParametro, object valor, Boolean parametroDeSalida = false, eTipo _tipo = eTipo.NULL)
        {
            try
            {
                DbParameter _objParam;
                _objParam = _objFactory.CreateParameter();
                _objParam.ParameterName = nombreParametro;

                valor = (valor == null) ? ValidarParametro(nombreParametro) : valor;

                _objParam.Value = valor.ToString();
                string _decision = valor.GetType().FullName;
                if (_tipo != eTipo.NULL)
                    _decision = getTipo(_tipo);
                switch (_decision)
                {
                    case "System.String":
                        _objParam.DbType = DbType.String;
                        break;
                    case "System.Int32":
                        _objParam.DbType = DbType.Int32;
                        break;
                    case "System.Decimal":
                        _objParam.DbType = DbType.Decimal;
                        break;
                    case "System.Single":
                        _objParam.DbType = DbType.Double;
                        break;
                    case "System.Date":
                        _objParam.DbType = DbType.Date;
                        break;
                    case "System.DateTime":
                        _objParam.DbType = DbType.DateTime;
                        break;
                    case "System.Boolean":
                        _objParam.DbType = DbType.Boolean;
                        break;
                }
                if (parametroDeSalida)
                    _objParam.Direction = ParameterDirection.Output;
                else
                    _objParam.Direction = ParameterDirection.Input;

                _objCommand.Parameters.Add(_objParam);
            }
            catch (Exception ex)
            {
                _strErrBase = ex.Message;
            }
        }

        private object ValidarParametro(string nombreParametro)
        {
            object _Validacion = new object();
            string _strConsulta = $"SELECT type_name(user_type_id) type  FROM sys.parameters WHERE object_id = object_id('{this._StoredProcedure}') AND name = '{nombreParametro}'";
            DataSet _ds = EjecutarQuery(_strConsulta);
            DataTable _dt = _ds.Tables[0];
            switch (_dt.Rows[0][0])
            {
                case "varchar":
                    _Validacion = string.Empty;
                    break;
                case "decimal":
                    _Validacion = new decimal();
                    break;
                case "int":
                    _Validacion = new int();
                    break;
                case "text":
                    _Validacion = string.Empty;
                    break;
                case "bit":
                    _Validacion = true;
                    break;
                case "float":
                    _Validacion = new float();
                    break;
                default:
                    _Validacion = string.Empty;
                    break;
            }
            return _Validacion;
        }
        public DataSet EjecutaStoredProcedure(string _strStored = "")
        {
            DataSet _executeStoredProcedure = new DataSet();
            _strStored = string.IsNullOrEmpty(_strStored) ? this._StoredProcedure : _strStored;
            try
            {
                _objConex.ConnectionString = _Connection;
                _objConex.Open();
            }
            catch (Exception ex)
            {
                _strErrBase = ex.Message;
            }
            if (_strErrBase.Trim() == string.Empty)
                try
                {
                    _objCommand.CommandText = _strStored;
                    _objCommand.CommandType = CommandType.StoredProcedure;
                    _objCommand.Connection = _objConex;
                    _objAdapter = _objFactory.CreateDataAdapter();
                    _objAdapter.SelectCommand = _objCommand;
                    _objAdapter.Fill(_executeStoredProcedure, "RESULTADO");
                    _objCommand.Parameters.Clear();
                    _objCommand.Connection.Close();
                }
                catch (Exception ex)
                {
                    _objCommand.Connection.Close();
                    _objCommand.Parameters.Clear();
                    _strErrBase = ex.Message;
                }
            return _executeStoredProcedure;
        }
        private string getTipo(eTipo _tipo)
        {
            return ((DescriptionAttribute[])_tipo.GetType().GetField(_tipo.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false))[0].Description;
        }
        private string getProveedor(eProveedor _proveedor)
        {
            return ((DescriptionAttribute[])_proveedor.GetType().GetField(_proveedor.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false))[0].Description;
        }
    }
    public enum ConnectionBD
    {
        BDUPAXER
    }
    public enum eTipo
    {
        [Description("")] NULL,
        [Description("System.Int32")] INT32,
        [Description("System.Date")] DATE,
        [Description("System.DateTime")] DATETIME,
        [Description("System.Boolean")] BOOL,
        [Description("System.String")] STRING,
        [Description("System.Single")] DOUBLE,
        [Description("System.Decimal")] DECIMAL
    }
    public enum eProveedor
    {
        [Description("System.Data.SqlClient")] SQLSERVER
    }
}
