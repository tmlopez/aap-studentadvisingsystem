using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OracleClient;
using edu.csu.chico.enr.data.OracleSQL;
using edu.csu.chico.enr.encryption.DataProtection;

namespace StudentAdvisingSystem
{
    public class DataClass
    {
        public string DecryptConnectionString(string EncryptedConnectionString)
        {
            DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);
            byte[] dataToDecrypt;
            dataToDecrypt = Convert.FromBase64String(EncryptedConnectionString);
            return System.Text.Encoding.ASCII.GetString(dp.Decrypt(dataToDecrypt, null));            
        }

        public DataSet ExecuteSQLStatement(string objType, string objName, string strSQL)
        {
            string strConnect;
            if (objType == "ENRDBR")
            {
                strConnect = DecryptConnectionString(ConfigurationManager.AppSettings.Get("ENRMGMT_ENR_ODS_ENRDBR").ToString());
                //strConnect = ConfigurationManager.AppSettings.Get("ENRMGMT_ENR_ODS_ENRDBR").ToString();
                SqlConnection objConnect = new SqlConnection(strConnect);
                DataSet objDataSet = new DataSet();

                objConnect.Open();
                SqlDataAdapter objDataAdapter = new SqlDataAdapter(strSQL, objConnect);
                objDataAdapter.Fill(objDataSet, objName);
                objConnect.Close();
                return objDataSet;
            }
            else if (objType == "ENRDBW")
            {
                strConnect = DecryptConnectionString(ConfigurationManager.AppSettings.Get("ENRMGMT_ENR_ODS_ENRDBW").ToString());
                //strConnect = ConfigurationManager.AppSettings.Get("ENRMGMT_ENR_ODS_ENRDBW").ToString();
                SqlConnection objConnect = new SqlConnection(strConnect);
                SqlCommand cmd = new SqlCommand(strSQL, objConnect);

                objConnect.Open();
                cmd.ExecuteScalar();
                objConnect.Close();
                return null;
            }
            else if (objType == "RDSDBR")
            {
                strConnect = DecryptConnectionString(ConfigurationManager.AppSettings.Get("RDSDB_ENRDBR").ToString());
                //strConnect = ConfigurationManager.AppSettings.Get("RDSDB_ENRDBR").ToString();
                DataSet objDataSet = OracleHelper.ExecuteDataset(strConnect, CommandType.Text, strSQL);
                if (objDataSet.Tables.Count == 1)
                {
                    objDataSet.Tables[0].TableName = objName;
                }
                return objDataSet;
            }
            else
            {
                return null;
            }
        }
    }
}