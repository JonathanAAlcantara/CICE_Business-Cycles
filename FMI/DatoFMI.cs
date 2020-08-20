using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FMI
{
    public class DatoFMI
    {
        public void GetDataFlow()
        {
            ServiceFMI.SDMXServiceClient cli = new ServiceFMI.SDMXServiceClient("CustomBinding_ISDMXService");

            System.Xml.Linq.XElement uno = cli.GetDataflow();


            string query =
            @"<QueryMessage xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message""> 
                <Query>
                    <KeyFamilyWhere xmlns=""http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query"">
                        <KeyFamily>FAS</KeyFamily>
                    </KeyFamilyWhere>
                </Query>
            </QueryMessage>";

            XDocument queryMessageDocument = XDocument.Parse(query);
            XElement queryMessage = queryMessageDocument.Root;

            var dos = cli.GetCodeList(queryMessage);

            string query1 =
            @"<QueryMessage xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
            xmlns=""http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message"">
              <Query>
                <DataWhere xmlns=""http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query"">
                  <And>
                    <DataSet>FAS</DataSet>
                    <Time>
                      <StartTime>2014</StartTime>
                      <EndTime>2015</EndTime>
                    </Time>
                    <Dimension id=""FREQ"">A</Dimension>
                    <Dimension id=""INDICATOR"">FCMOA_NUM</Dimension>
                    <!-- Geographical Outreach, Mobile Banking, Agent Outlets, active, Number of -->
                    <Dimension id=""REF_AREA"">AF</Dimension>
                    <!-- Afghanistan, Islamic Republic of -->
                  </And>
                </DataWhere>
              </Query>
            </QueryMessage>";

            queryMessageDocument = XDocument.Parse(query1);
            queryMessage = queryMessageDocument.Root;

            var tres = cli.GetCompactData(queryMessage);

            var cuatro = cli.GetDataStructure(queryMessage);

            var cinco = cli.GetMaxSeriesInResult();

            var seis = cli.GetMetadataStructure(queryMessage);
        }
    }
}
