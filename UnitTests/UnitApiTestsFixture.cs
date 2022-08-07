using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using IntegrationManagerCallApiService;
using Moq;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.DTO;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Contract;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Asset;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Asset;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Invoice;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.IntegrationManagerModels;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Invoice;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Parent;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.Xml;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Parent;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Dealer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Dealer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Message;
using Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Customer;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Customer;

namespace IDSIM.Services.UnitTest
{
    [CollectionDefinition(nameof(UnitApiTestsCollection))]
    public class UnitApiTestsCollection : ICollectionFixture<UnitApiTestsFixture>
    {
    }

    [ExcludeFromCodeCoverage]
    public class UnitApiTestsFixture : IDisposable
    {
        private List<Region>? _validRegions { get; set; }

        public Mock<IWebHostEnvironment> GetEnvironmentMock()
        {
            var hostEnvironment = new Mock<IWebHostEnvironment>();
            hostEnvironment.Setup(h => h.EnvironmentName).Returns("Testing");

            return hostEnvironment;
        }

        public Region GetValidRegion()
        {
            var random = new Random();

            if (_validRegions != null)
            {
                return _validRegions[random.Next(_validRegions.Count)];
            }

            var validRegions = new List<Region>();
            var regionFields = typeof(Region).GetFields();

            for (int i = 0; i < regionFields.Length; i++)
            {
                var value = regionFields[i].GetValue(null) as Region;

                if (value != null)
                {
                    var isActive = random.NextDouble() > 0.5;

                    if (i == regionFields.Length - 1 && validRegions.Count == 0)
                    {
                        isActive = true;
                    }

                    var region = new Region(value.Id, value.Name, isActive);

                    if (region.IsActive)
                    {
                        validRegions.Add(region);
                    }
                }
            }

            _validRegions = validRegions;

            return _validRegions[random.Next(_validRegions.Count)];
        }

        public Region GetValidRegionByName(string name)
        {
            var regionFields = typeof(Region).GetFields();

            for (int i = 0; i < regionFields.Length; i++)
            {
                var value = regionFields[i].GetValue(null) as Region;

                if (value != null && value?.Name == name)
                {
                    return new Region(value.Id, value.Name, true);
                }
            }

            return new Region(9999, "Testing Valid Region", true);
        }

        public Region GetInvalidRegion()
        {
            return new Region(-1, "Invalid Region", false);
        }

        public WebServiceCallReturnDataDto? GetValidFaultReturnDataDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Body = new WebServiceCallDataBodyDto()
                {
                    Fault = GetValidWebServiceCallFaultDto()
                }
            };
        }

        public WebServiceCallFaultDto GetValidWebServiceCallFaultDto()
        {
            return new WebServiceCallFaultDto()
            {
                Faultcode = "Test Faultcode",
                Faultstring = "Test Faultstring",
                FaultSection = "Test FaultSection",
                Detail = new WebServiceCallReturnDetail()
                {
                    Idsgrpdetail = new WebServiceCallIdsgrpdetailDto()
                    {
                        Code = "Test DetailCode",
                        Level = "Test DetailLevel",
                        Message = "Test DetailMessage"
                    }
                }
            };
        }

        public webServiceCallResponse GetValidInfoLeaseVersionResponse()
        {
            return new webServiceCallResponse(
             @$"<Envelope>
                    <Header>
                    <ApiName>IlCCGetInfoLeaseVersion</ApiName>
                    </Header>
                    <Body>
                        <Response>
                            <Parameter pk=""0"">
                                <ParamILSubRelease>IL.9F-385.25</ParamILSubRelease>
                                <ParamSysRelease>SYS.9F-61</ParamSysRelease>
                            </Parameter>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public WebServiceCallReturnDataDto? GetValidInfoLeaseVersionDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "IlCCGetInfoLeaseVersion"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Parameter = new WebServiceCallParameterDto()
                        {
                            Pk = "0",
                            ParamILSubRelease = "IL.9F-385.26",
                            ParamSysRelease = "SYS.9F-61"
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetFaultedInfoLeaseVersionDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "IlCCGetInfoLeaseVersion"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Fault = new WebServiceCallFaultDto()
                    {
                        Faultstring = "Testing Integration Manager internal error"
                    }
                }
            };
        }

        public FacadeResponse<SearchContractsResponseModel> GetValidSearchContractsResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchContractsResponseModel>()
            {
                Data = CsKeySerializer.SearchContractsResponseFromString(GetValidSearchContractsResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<GetContractsByCCANResponseModel> GetValidGetContractsByCCANResponseModelFacadeResponse()
        {
            return new FacadeResponse<GetContractsByCCANResponseModel>()
            {
                Data = CsKeySerializer.GetContractsByCCANResponseFromString(GetValidGetContractsByCCANResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<ReserveContractNumberResponseModel> GetValidReserveContractNumberResponseModelFacadeResponse()
        {
            return new FacadeResponse<ReserveContractNumberResponseModel>()
            {
                Data = CsKeySerializer.ReserveContractNumberResponseFromString(GetValidReserveContractNumberResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<GetContractLinkCodeDetailsByCCANResponseModel> GetValidGetContractLinkCodeDetailsByCCANResponseModelFacadeResponse()
        {
            return new FacadeResponse<GetContractLinkCodeDetailsByCCANResponseModel>()
            {
                Data = CsKeySerializer.GetContractLinkCodeDetailsByCCANResponseFromString(GetValidGetContractLinkCodeDetailsByCCANResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<ListParentsResponseModel> GetValidListParentsResponseModelFacadeResponse()
        {
            var result = GetValidListParentsResponseDto();

            var listParentCode = result?.Body?.Response?.ParentCode?.ToList();
            var count = listParentCode?.Count ?? 0;
            var statusCode = count > 0 ? 1 : 0;
            var message = statusCode == 1 ? $"Parent Codes found: {count}" : "No Parent Codes found";

            return new FacadeResponse<ListParentsResponseModel>()
            {
                Data = new ListParentsResponseModel()
                {
                    StatusCode = statusCode,
                    Message = message,
                    ParentCodeList = listParentCode?.ConvertAll(x =>
                    {
                        int.TryParse(x.CsPcKey, out int key);

                        return new ListParentsParentCodeListResponseModel()
                        {
                            Key = key,
                            Description = x.CsPcDesc,
                            Reference = x.CsPcReference
                        };
                    })
                }
            };
        }

        public FacadeResponse<GetContractResponseModel> GetValidGetContractResponseModelFacadeResponse()
        {
            return new FacadeResponse<GetContractResponseModel>()
            {
                Data = CsKeySerializer.GetContractResponseFromString(GetValidGetContractResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<CreateParentResponseModel> GetValidCreateParentResponseModelFacadeResponse()
        {
            return new FacadeResponse<CreateParentResponseModel>()
            {
                Data = CsKeySerializer.CreateParentResponseFromString(GetValidCreateParentResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<SearchDealerResponseModel> GetValidSearchDealerResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchDealerResponseModel>()
            {
                Data = CsKeySerializer.SearchDealerResponseFromString(GetValidSearchDealerResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<CreateDealerResponseModel> GetValidCreateDealerResponseModelFacadeResponse()
        {
            return new FacadeResponse<CreateDealerResponseModel>()
            {
                Data = CsKeySerializer.CreateDealerResponseFromString(GetValidCreateDealerResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<SearchAssetsResponseModel> GetValidSearchAssetsResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchAssetsResponseModel>()
            {
                Data = CsKeySerializer.SearchAssetsResponseFromString(GetValidSearchAssetsResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<SearchMessagesResponseModel> GetValidSearchMessagesResponseModelFacadeResponse()
        {
            int rangeBegin = 1, rangeEnd = 10;
            List<CommentDataModel>? messages = new List<CommentDataModel>();
            WebServiceCallMessageResponseDto[] messagesdto = GetValidMessageArray(rangeBegin, rangeEnd);
            foreach (var message in messagesdto)
            {
                messages.Add(new CommentDataModel(message));
            }
            return new FacadeResponse<SearchMessagesResponseModel>()
            {
                Data = new SearchMessagesResponseModel()
                {
                    Comments = messages,
                    Total = messages.Count.ToString(),
                    MessageStatus = new MessageStatus()
                    {
                        code = "0",
                        message = " records have been found. Searching from " + rangeBegin + " to " + rangeEnd + "."
                    }
                }
            };
        }

        public FacadeResponse<CreateMessageResponseModel> GetValidCreateMessageResponseModelFacadeResponse()
        {           
            return new FacadeResponse<CreateMessageResponseModel>()
            {
                Data = new CreateMessageResponseModel()
                {
                    code = "0",
                    message = "Comment Saved."
                }
            };
        }

        public FacadeResponse<SearchCustomerResponseModel> GetValidSearchCustomerResponseModelFacadeResponse()
        {
            var CustomersList = GetValidCustomerListResponse();
            return new FacadeResponse<SearchCustomerResponseModel>()
            {
                Data = new SearchCustomerResponseModel()
                {
                    StatusCode = 1,
                    Message = "Customer(s) found:  " + CustomersList.Count,
                    Customers = CustomersList
                }
            };
        }

        public FacadeResponse<CreateCustomerResponseModel> GetValidCreateCustomerResponseModelFacadeResponse()
        {
            return new FacadeResponse<CreateCustomerResponseModel>()
            {
                Data = getValidCreateCustomerResponseModel()
            };
        }
        public GetContractResponseModel GetValidGetContractResponseModel()
        {
            return CsKeySerializer.GetContractResponseFromString(GetValidGetContractCsKey());
        }

        public FacadeResponse<SearchInvoicesResponseModel> GetValidSearchInvoicesResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchInvoicesResponseModel>()
            {
                Data = CsKeySerializer.SearchInvoicesResponseFromString(GetValidSearchInvoicesResponseDto()?.Body?.Response?.Customer?.CsKey, GetValidGetContractResponseModel())
            };
        }

        public FacadeResponse<UpdatePromiseToPayDateResponseModel> GetValidUpdatePromiseToPayDateResponseModelFacadeResponse()
        {
            return new FacadeResponse<UpdatePromiseToPayDateResponseModel>()
            {
                Data = CsKeySerializer.UpdatePromiseToPayDateResponseFromString(GetValidUpdatePromiseToPayDateResponseDto()?.Body?.Response?.Customer?.CsKey)
            };
        }

        public FacadeResponse<SearchContractsResponseModel> GetInvalidSearchContractsResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchContractsResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<ListParentsResponseModel> GetInvalidListParentsResponseModelFacadeResponse()
        {
            return new FacadeResponse<ListParentsResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<GetContractsByCCANResponseModel> GetInvalidGetContractsByCCANResponseModelFacadeResponse()
        {
            return new FacadeResponse<GetContractsByCCANResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<ReserveContractNumberResponseModel> GetInvalidReserveContractNumberResponseModelFacadeResponse()
        {
            return new FacadeResponse<ReserveContractNumberResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<GetContractLinkCodeDetailsByCCANResponseModel> GetInvalidGetContractLinkCodeDetailsByCCANResponseModelFacadeResponse()
        {
            return new FacadeResponse<GetContractLinkCodeDetailsByCCANResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<GetContractResponseModel> GetInvalidGetContractResponseModelFacadeResponse()
        {
            return new FacadeResponse<GetContractResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<CreateParentResponseModel> GetInvalidCreateParentResponseModelFacadeResponse()
        {
            return new FacadeResponse<CreateParentResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<SearchDealerResponseModel> GetInvalidSearchDealerResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchDealerResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<CreateDealerResponseModel> GetInvalidCreateDealerResponseModelFacadeResponse()
        {
            return new FacadeResponse<CreateDealerResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<SearchAssetsResponseModel> GetInvalidSearchAssetsResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchAssetsResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<SearchInvoicesResponseModel> GetInvalidSearchInvoicesResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchInvoicesResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<UpdatePromiseToPayDateResponseModel> GetInvalidUpdatePromiseToPayDateResponseModelFacadeResponse()
        {
            return new FacadeResponse<UpdatePromiseToPayDateResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }


        public FacadeResponse<SearchMessagesResponseModel> GetInvalidSearchMessagesResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchMessagesResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<CreateMessageResponseModel> GetInvalidCreateMessageResponseModelFacadeResponse()
        {
            return new FacadeResponse<CreateMessageResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public FacadeResponse<SearchCustomerResponseModel> GetInvalidSearchCustomerResponseModelFacadeResponse()
        {
            return new FacadeResponse<SearchCustomerResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }
        
        public FacadeResponse<CreateCustomerResponseModel> GetInvalidCreateCustomerResponseModelFacadeResponse()
        {
            return new FacadeResponse<CreateCustomerResponseModel>()
            {
                Errors = new List<string?>() { { ErrorMessages.RegionOperationNotAllowed } }
            };
        }

        public webServiceCallResponse GetInvalidRegionInfoLeaseVersionResponse(string? pDbID)
        {
            return new webServiceCallResponse(
                @$"<Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                    <Body>
                        <Fault>
                            <faultcode>Server.userException</faultcode>
                            <faultstring>com.idsgrp.ilconnect.pool.PoolException: {pDbID} is not a valid database pool name.</faultstring>
                            <detail>
                                <ns1:hostname xmlns:ns1=""http://xml.apache.org/axis/"">dimnldevmanagd1.us.dell.com</ns1:hostname>
                            </detail>
                        </Fault>
                    </Body>
                </Envelope>");
        }

        public SearchRequestModel GetValidSearchRequestModel()
        {
            return new SearchRequestModel()
            {
                CCAN = "2002"
            };
        }

        public GetContractDetailsRequestModel GetValidGetContractDetailsModel()
        {
            return new GetContractDetailsRequestModel()
            {
                ContractNumber = "203-0138542-002"
            };
        }

        public CreateParentViewRequestModel GetValidCreateParentViewRequestModel()
        {
            return new CreateParentViewRequestModel()
            {
                ParentCode = new CreateParentParentCodeViewRequestModel()
                {
                    Description = "Testing Parent",
                    Reference = "Testing Parent Reference"
                }
            };
        }

        public CreateParentRequestModel GetValidCreateParentRequestModel()
        {
            return new CreateParentRequestModel()
            {
                Region = GetValidRegion(),
                Description = "Testing Parent",
                Reference = "Testing Parent Reference"
            };
        }

        public SearchCustomerRequestModel GetValidSearchCustomerRequestModel() {

            return new SearchCustomerRequestModel()
            {
                Customer = new SearchCustomerRequestCustomerModel() { 
                    Name = "DAN",
                    ShortName = String.Empty,
                    DBA = String.Empty,
                    ContactPhone = String.Empty,
                    FederalID = String.Empty
                },
                SearchType = "S" 
            };
        
        }

        public CreateCustomerRequestModel GetValidCreateCustomerRequestModel()
        {

            return new CreateCustomerRequestModel()
            {                
                Customer = new CustomerRequestDataModel()
                {
                    Name = "TEST_NAME",
                    DBA = "TEST_DBA",
                    ShortName = "TEST_SHORT_NAME",
                    Address1 = "TEST_ADD1",
                    Address2 = "TEST_ADD2",
                    Address3 = "TEST_ADD3",
                    City = "Teutonia",
                    State = "RS",
                    Zip = "95890000",
                    Country = "BR",
                    ARName = "ARNAME",
                    ARAddress1 = "ARADD1",
                    ARAddress2 = "ARADD2",
                    ARAddress3 = "ARADD3",
                    ARCity = "Manaus",
                    ARCountry = "BR",
                    ARState = "AM",
                    ARZip = "69040000",
                    ARAttn = "SENOJ",
                    ContactName = "BETO",
                    ContactPhone = "+550800393939",
                    ContactEmail = "jones@kgb.cz",
                    ContactFaxPhone = "1234455",
                    Title = "Mr",
                    BankNumber = "125683",
                    BankAccountNumber = "12",
                    Dealer = "12",
                    InsuranceAgent = "23123",
                    FederalID = "TEST_FEDERAL_ID",
                    Language = "E_LG",
                    SicCode = 4444,
                    BusinessType = 22,
                    BirthDate = DateTime.Today,
                    BusinessDesc = "",
                    BeginBusinessYear = 444,
                    RelatedCustomer = "0123456789",
                    TaxExemptNumber = "",
                    CreditScoring = "CRSC",
                    FacilityScore = "FASC",
                    InvoiceDueDay = 12,
                    ParentCode = 1122334455,
                    Business = 15,
                    BusinessPhone = "",
                    Collector = 4444,
                    MlaNumber = "",
                    MlaDate = "",
                    ObligorScore = 777999,
                    CompanyRegistration = "888666",
                    CreditBand = "",
                    RelatedCodes = 22,
                    ACDCSValue = 10,
                    ACDCSEffDate = DateTime.Today,
                    ACDCSSourceInd = "B",
                    ResidentInCountry = true                   

                }
            };

        }        

        public SearchDealerViewRequestModel GetValidSearchDealerViewRequestModel()
        {
            return new SearchDealerViewRequestModel()
            {
                SearchType = "S",
                Dealer = new SearchDealerDealerViewRequestModel()
                {
                    Name = "LIQUID IT LIMITED",
                    Type = "DVM",
                    ContactPhone = "0000"
                }
            };
        }

        public CreateDealerViewRequestModel GetValidCreateDealerViewRequestModel()
        {
            return new CreateDealerViewRequestModel()
            {
                Dealer = new CreateDealerDealerViewRequestModel()
                {
                    Prefix = "+",
                    Suffix = "0000",
                    PrefixStart = 1,
                    SuffixStart = 1,
                    Name = "Test Dealer 1",
                    Type = "D",
                    Address1 = "Test Address 1",
                    Address2 = "Test Address 2",
                    City = "Test City",
                    State = "Test State",
                    Zip = "Test Zip",
                    Country = "Test Country",
                    ContactName = "Test Contact Name",
                    ContactPhone = "Test Contact Phone",
                    Fax = "Test Fax",
                    PaymentPlan = "Test Payment Plan",
                    Model = "Test Model"
                }
            };
        }

        public ReserveContractNumberRequestModel GetValidReserveContractNumberRequestModel(bool withMiddle = true)
        {
            return new ReserveContractNumberRequestModel()
            {
                Region = GetValidRegion(),
                Lessor = "009",
                Middle = withMiddle ? "0138521" : null
            };
        }

        public SearchDealerRequestModel GetValidSearchDealerRequestModel()
        {
            return new SearchDealerRequestModel()
            {
                Region = GetValidRegion(),
                SearchType = "S",
                Name = "LIQUID IT LIMITED",
                Type = "DVM",
                ContactPhone = "0000"
            };
        }

        public CreateDealerRequestModel GetValidCreateDealerRequestModel()
        {
            return new CreateDealerRequestModel()
            {
                Region = GetValidRegion(),
                Prefix = "+",
                Suffix = "0000",
                PrefixStart = 1,
                SuffixStart = 1,
                Name = "Test Dealer 1",
                Type = "D",
                Address1 = "Test Address 1",
                Address2 = "Test Address 2",
                City = "Test City",
                State = "Test State",
                Zip = "Test Zip",
                Country = "Test Country",
                ContactName = "Test Contact Name",
                ContactPhone = "Test Contact Phone",
                Fax = "Test Fax",
                PaymentPlan = "Test Payment Plan",
                Model = "Test Model"
            };
        }

        public GetContractLinkCodeDetailsByCCANRequestModel GetValidGetContractLinkCodeDetailsByCCANRequestModel()
        {
            return new GetContractLinkCodeDetailsByCCANRequestModel()
            {
                Region = GetValidRegion(),
                CCAN = "2010050100"
            };
        }

        public CreateParentRequestModel GetErrorCreateParentRequestModel()
        {
            return new CreateParentRequestModel()
            {
                Region = GetValidRegion()
            };
        }

        public ReserveContractNumberRequestModel GetErrorReserveContractNumberRequestModel()
        {
            return new ReserveContractNumberRequestModel()
            {
                Region = GetValidRegion()
            };
        }

        public SearchByCustomerRequestModel GetValidSearchCustomerContractModel()
        {
            return new SearchByCustomerRequestModel()
            {
                CCAN = "2005",
                PagedRequest = new Pagination()
                {
                    StartIndex = 1,
                    EndIndex = 10,
                    SortType = "DESC",
                    SortField = "CONTRACT"
                }
            };
        }
        public SearchMessagesIMRequestModel GetValidSearchMessagesIMRequestModel()
        {
            return new SearchMessagesIMRequestModel()
            {
                Region = GetValidRegion(),
                ContractNumber = "006-0027796-005",
                CCAN = "100687",
                Pagination = new Pagination()
                {
                    StartIndex = 1,
                    EndIndex = 10
                }
            };
        }

        public CreateMessageRequestModel GetValidCreateMessageRequestModel()
        {
            return new CreateMessageRequestModel()
            {
                Comment = new CreateCommentDataModel()
                {
                    ContractNumber = "001-6093966-001",
                    Department = "01",
                    Name = "Mr. Unit Test",
                    CaseNumber = "001",
                    Type = "1",
                    Comment = "Just testing the Comment",
                }
            };
        }

        public WebServiceCallMessageResponseDto[] GetValidWebServiceCallMessageResponseDto() {
            return new WebServiceCallMessageResponseDto[] { new WebServiceCallMessageResponseDto() {
                    pk = "1988864",
                    pos = "1",
                    maxpos = "4",
                    MsgDate = "2000-04-14",
                    MsgKey = "1988864",
                    MsgMessage = "ASSIGNED TO NEWTON ON 041500.....DES",
                    MsgName = "JOHN Q PUBLIC -T",
                    MsgTime = "12:50:54",
                    MsgDept = "01",
                    MsgMasterKey = "001-6093966-001",
                    MsgMessageType = 1} 
            };
        }

        public CreateMessageIMRequestModel GetValidCreateMessageIMRequestModel()
        {
            return new CreateMessageIMRequestModel()
            {
                IsAsync = false,
                Region = GetValidRegion(),
                Comment = new CommentDataModel(GetValidWebServiceCallMessageResponseDto()[0])
            };
        }

        public Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Contract.GetContractsByCCANRequestModel GetValidApiGetContractsByCCANRequestModel()
        {
            return new Dell.DFS.LLS.IntegrationManagerServices.Api.Models.Contract.GetContractsByCCANRequestModel()
            {
                CCAN = "287746"
            };
        }

        public Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract.GetContractsByCCANRequestModel GetValidApiGatewayGetContractsByCCANRequestModel()
        {
            return new Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract.GetContractsByCCANRequestModel()
            {
                Region = GetValidRegionByName(Region.EMEA.ToString()),
                CCAN = "287746",
                SearchType = "Middle"
            };
        }

        public ReserveContractNumberViewRequestModel GetValidReserveContractNumberViewRequestModel()
        {
            return new ReserveContractNumberViewRequestModel()
            {
                Contract = new ReserveContractNumberContractViewRequestModel()
                {
                    Lessor = "009",
                    Middle = "0138521"
                }
            };
        }

        public GetContractLinkCodeDetailsByCCANViewRequestModel GetValidGetContractLinkCodeDetailsByCCANViewRequestModel()
        {
            return new GetContractLinkCodeDetailsByCCANViewRequestModel()
            {
                CCAN = "2010050100"
            };
        }

        public AssetSearchByContractRequestModel GetValidAssetSearchByContractRequestModel()
        {
            return new AssetSearchByContractRequestModel()
            {
                ContractNumber = "001-6628692-501"
            };
        }

        public InvoiceSearchByContractRequestModel GetValidInvoiceSearchByContractRequestModel()
        {
            return new InvoiceSearchByContractRequestModel()
            {
                ContractNumber = "203-0138542-002",
                PastDueOnly = "Y",
                Pagination = new Pagination()
                {
                    StartIndex = 0,
                    EndIndex = 25,
                    SortField = "DUE_DATE",
                    SortType = "DESC"
                }
            };
        }

        public UpdatePromiseToPayRequestModel GetValidUpdatePromiseToPayRequestModel()
        {
            return new UpdatePromiseToPayRequestModel()
            {
                SfdcPortalUsername = "Contact E2Etest",
                CCAN = "290095",
                ContractNumber = "203-0138542-001",
                InvoiceNumber = "9300035663",
                PromiseToPayDate = "2022-06-28"
            };
        }

        public UpdatePromiseToPayDateRequestModel GetValidUpdatePromiseToPayDateRequestModel()
        {
            return new UpdatePromiseToPayDateRequestModel()
            {
                Region = GetValidRegion(),
                SfdcPortalUsername = "Contact E2Etest",
                CCAN = "290095",
                ContractNumber = "203-0138542-001",
                InvoiceNumber = "9300035663",
                PromiseToPayDate = "2022-06-28"
            };
        }

        public SearchContractsRequestModel GetValidSearchContractsRequestModel()
        {
            return new SearchContractsRequestModel()
            {
                Region = GetValidRegion(),
                ContractNumber = "001-6628692-501",
                CCAN = "2002",
                PhoneNumber = "123456789",
                CreditApplicationNumber = "12345678",
                SerialTagNumber = "1234567",
                InvoiceNumber = "123456",
                CustomerName = "TEST CUSTOMER",
                ContactName = "TEST CONTACT NAME",
                DellOrderNumber = "12345",
                FedID = "1234",
                PONumber = "123",
                TermDays = "12",
                Paged = "Y",
                Pagination = new Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.IntegrationManagerModels.Pagination()
                {
                    StartIndex = 1,
                    SortType = "DESC",
                    EndIndex = 10
                }
            };
        }

        public ListParentsRequestModel GetValidListParentsRequestModel()
        {
            return new ListParentsRequestModel()
            {
                Region = GetValidRegion()
            };
        }

        public SearchAssetsRequestModel GetValidSearchAssetsRequestModel()
        {
            return new SearchAssetsRequestModel()
            {
                Region = GetValidRegion(),
                ContractNumber = "001-6628692-501",
                CCAN = "1234",
                DellOrderNumber = "12345",
                SerialNumber = "1234567",
                TermBeginDate = "01/01/2017",
                TermEndDate = "12/31/2017",
                EquipmentZipCode = 123456,
                Pagination = new Pagination()
                {
                    StartIndex = 1,
                    SortType = "DESC",
                    EndIndex = 10
                }
            };
        }

        public SearchInvoicesRequestModel GetValidSearchInvoicesRequestModel()
        {
            return new SearchInvoicesRequestModel()
            {
                Region = GetValidRegion(),
                ContractNumber = "201-0090326-160",
                PastDueOnly = "Y",
                Pagination = new Pagination()
                {
                    StartIndex = 0,
                    SortField = "DUE_DATE",
                    SortType = "DESC",
                    EndIndex = 25
                }
            };
        }

        public GetContractRequestModel GetValidGetContractRequestModel()
        {
            return new GetContractRequestModel()
            {
                Region = GetValidRegion(),
                ContractNumber = "201-0090326-160"
            };
        }

        public SearchContractsRequestModel GetInvalidSearchContractsRequestModel()
        {
            return new SearchContractsRequestModel()
            {
                Region = GetInvalidRegion(),
                ContractNumber = "1234",
                CCAN = "ABC",
                Pagination = new Pagination()
                {
                    StartIndex = 0,
                    EndIndex = 10,
                    SortField = "Undefined",
                    SortType = "Undefined"
                }
            };
        }

        public ListParentsRequestModel GetInvalidListParentsRequestModel()
        {
            return new ListParentsRequestModel()
            {
                Region = GetInvalidRegion()
            };
        }

        public Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract.GetContractsByCCANRequestModel GetInvalidApiGatewayGetContractsByCCANRequestModel()
        {
            return new Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract.GetContractsByCCANRequestModel()
            {
                Region = GetInvalidRegion()
            };
        }

        public CreateParentRequestModel GetInvalidCreateParentRequestModel()
        {
            return new CreateParentRequestModel()
            {
                Region = GetInvalidRegion()
            };
        }

        public ReserveContractNumberRequestModel GetInvalidReserveContractNumberRequestModel()
        {
            return new ReserveContractNumberRequestModel()
            {
                Region = GetInvalidRegion(),
                Lessor = "1234",
                Middle = "12345678"
            };
        }

        public SearchDealerRequestModel GetInvalidSearchDealerRequestModel()
        {
            return new SearchDealerRequestModel()
            {
                Region = GetInvalidRegion(),
                SearchType = "b",
                Name = "Invalid Name",
                Type = "c",
                ContactPhone = "0000"
            };
        }

        public CreateDealerRequestModel GetInvalidCreateDealerRequestModel()
        {
            return new CreateDealerRequestModel()
            {
                Region = GetInvalidRegion(),
                Type = "B"
            };
        }

        public GetContractLinkCodeDetailsByCCANRequestModel GetInvalidGetContractLinkCodeDetailsByCCANRequestModel()
        {
            return new GetContractLinkCodeDetailsByCCANRequestModel()
            {
                Region = GetInvalidRegion(),
                CCAN = "B"
            };
        }

        public SearchAssetsRequestModel GetInvalidSearchAssetsRequestModel()
        {
            return new SearchAssetsRequestModel()
            {
                Region = GetInvalidRegion(),
                ContractNumber = "1234",
                CCAN = "ABC",
                TermBeginDate = "12345",
                TermEndDate = "123456",
                Pagination = new Pagination()
                {
                    StartIndex = 0,
                    EndIndex = 10,
                    SortField = "Undefined",
                    SortType = "Undefined"
                }
            };
        }

        public SearchInvoicesRequestModel GetInvalidSearchInvoicesRequestModel()
        {
            return new SearchInvoicesRequestModel()
            {
                Region = GetInvalidRegion(),
                ContractNumber = "1234",
                Category = "ABC",
                PastDueOnly = "ABCD",
                Pagination = new Pagination()
                {
                    StartIndex = 0,
                    EndIndex = 10,
                    SortField = "Undefined",
                    SortType = "Undefined"
                }
            };
        }

        public GetContractRequestModel GetInvalidGetContractRequestModel()
        {
            return new GetContractRequestModel()
            {
                Region = GetInvalidRegion(),
                ContractNumber = "1234"
            };
        }


        public SearchMessagesIMRequestModel GetInvalidSearchMessagesIMRequestModel()
        {
            return new SearchMessagesIMRequestModel()
            {
                Region = GetInvalidRegion(),
                ContractNumber = "1234"
            };
        }

        public CreateMessageIMRequestModel GetInvalidCreateMessageIMRequestModel()
        {
            return new CreateMessageIMRequestModel()
            {
                Region = GetInvalidRegion(),
                Comment = new CommentDataModel() { 
                    Key = "12324",
                    Type = 1,
                    DateTime = DateTime.Now,
                    Department = "XX",
                    Name = "INVALID"
                }
            };
        }               

        public UpdatePromiseToPayDateRequestModel GetInvalidUpdatePromiseToPayDateRequestModel()
        {
            return new UpdatePromiseToPayDateRequestModel()
            {
                Region = GetInvalidRegion(),
                CCAN = "abc",
                SfdcPortalUsername = "Invalid User",
                ContractNumber = "12345",
                InvoiceNumber = "1234",
                PromiseToPayDate = "111-1-1"
            };
        }

        public WebServiceCallReturnDataDto? GetValidSearchContractsResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr2"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = "0|~1 Contracts have been found.|~1|~1|~2002|~|~MY NAME IS: 2002|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~|~Mexico|~007|~11700|~301-0000004-001|~1023|~0008|~023000|~0013|~MY NAME IS: 301-0000004-001|~JOHN Q PUBLIC -THIS|~RL|~36|~02/01/2015|~02/01/2018|~|~USD|~0.00|~|~1|~27|~0102|~|~N|~0|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~Mexico|~007|~MY NAME IS: 301-0000004-001|~CDX|~007|~11700|~|~404|~XAXX010101000|~Various|~0|~0.00|~0.00|~0.00|~0.00|~0.00|~|~|~|~|~0.00|~|~02/01/2018|~0"
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidGetContractsByCCANResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidGetContractsByCCANCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidReserveContractNumberResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidReserveContractNumberCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidGetContractLinkCodeDetailsByCCANResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidGetContractLinkCodeDetailsByCCANCsKey()
                        }
                    }
                }
            };
        }

        public string GetValidGetContractsByCCANCsKey()
        {
            return "1|~287746|~203-0136331|~EUR";
        }

        public string GetValidReserveContractNumberCsKey()
        {
            return "1|~009-0145591-001";
        }

        public string GetValidGetContractLinkCodeDetailsByCCANCsKey()
        {
            return "1|~2010050100|~1|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~BASINGTOKE |~CO.|~RG21 3NB|~1|~60|~A|~GIL";
        }

        public WebServiceCallReturnDataDto? GetValidListParentsResponseDto()
        {
            return CustomXmlSerializer.DeserializeFromString<WebServiceCallReturnDataDto?>(GetValidListParentsResponse().webServiceCallReturn);
        }

        public WebServiceCallReturnDataDto? GetValidSearchAssetsResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr2"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidSearchAssetCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidSearchInvoicesResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr2"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidSearchInvoiceCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidUpdatePromiseToPayDateResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr2"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidUpdatePromiseToPayDateCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidGetContractResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr2"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidGetContractCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidCreateParentResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidCreateParentCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidSearchDealerResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidSearchDealerCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidCreateDealerResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetValidCreateDealerCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidSearchMessagesResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaListMessages"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Message = GetValidMessageArray(1, 30)
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetValidCreateMessageResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaCreateMessage"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Message = GetValidWebServiceCallMessageResponseDto()
                    }
                }
            };
        }

        

        public WebServiceCallReturnDataDto? GetErrorCreateParentResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetErrorCreateParentCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetErrorReserveContractNumberResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = GetErrorReserveContractNumberCsKey()
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetErrorSearchDealerResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = string.Empty
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetErrorCreateDealerResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = string.Empty
                        }
                    }
                }
            };
        }

        public WebServiceCallReturnDataDto? GetErrorGetContractLinkCodeDetailsByCCANResponseDto()
        {
            return new WebServiceCallReturnDataDto()
            {
                Header = new WebServiceCallDataHeaderDto()
                {
                    ApiName = "UdaDellCallSubr"
                },
                Body = new WebServiceCallDataBodyDto()
                {
                    Response = new WebServiceCallResponseDto()
                    {
                        Customer = new WebServiceCallCustomerDto()
                        {
                            CsKey = "99|~Test error"
                        }
                    }
                }
            };
        }

        public string GetValidSearchAssetCsKey()
        {
            return "0|~15 Assets have been found for contract 001-6628692-501|~15|~1|^2|^3|^4|^5|^6|^7|^8|^9|^10|^11|^12|^13|^14|^15|~33660411|^33660412|^33660413|^33660414|^33660415|^33660416|^33660417|^33660418|^33660419|^33660420|^33660421|^33660422|^33660423|^33660424|^33660425|~Dell OptiPlex 7010 SFF Base, FT,V1|^Dell OptiPlex 7010 SFF Base, FT,V1|^Dell OptiPlex 7010 SFF Base, FT,V1|^Dell OptiPlex 7010 SFF Base, FT,V1|^Dell OptiPlex 7010 SFF Base, FT,V1|^Dell Opti 3010 SFF Base, (FCG5)|^Dell Opti 3010 SFF Base, (FCG5)|^Dell Opti 3010 SFF Base, (FCG5)|^Dell Opti 3010 SFF Base, (FCG5)|^Dell Opti 3010 SFF Base, (FCG5)|^Dell OptiPlex 7010 MT Base, FT,V1|^Dell OptiPlex 7010 MT Base, FT,V1|^Dell OptiPlex 7010 MT Base, FT,V1|^Dell OptiPlex 7010 MT Base, FT,V1|^Dell OptiPlex 7010 MT Base, FT,V1|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|^CLOSED|~111JXV1|^10YCXV1|^10YHXV1|^111HXV1|^113GXV1|^14Z7NV1|^7TZ7NV1|^9TZ7NV1|^CSZ7NV1|^6VZ7NV1|^CQ1JVV1|^CQJMVV1|^CRYMVV1|^CPBKVV1|^CQQNVV1|~225-3211|^225-3211|^225-3211|^225-3211|^225-3211|^225-4091|^225-4091|^225-4091|^225-4091|^225-4091|^225-3205|^225-3205|^225-3205|^225-3205|^225-3205|~634.68|^634.70|^634.70|^634.70|^634.70|^645.65|^645.65|^645.65|^645.65|^645.65|^601.85|^601.83|^601.83|^601.83|^601.83|~6.74625299|^6.74297334|^6.74297334|^6.74297334|^6.74297334|^6.86104096|^6.86104096|^6.86104096|^6.86104096|^6.86104096|^6.39532977|^6.39532977|^6.39532977|^6.39532977|^6.39532977|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~008|^008|^008|^008|^008|^008|^008|^008|^008|^008|^008|^008|^008|^008|^008|~03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|~0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|^0000010000|~0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~01|^01|^01|^01|^01|^01|^01|^01|^01|^01|^01|^01|^01|^01|^01|~957|^957|^957|^957|^957|^957|^957|^957|^957|^957|^957|^957|^957|^957|^957|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|^CORPUS CHRISTI|~001|^001|^001|^001|^001|^001|^001|^001|^001|^001|^001|^001|^001|^001|^001|~78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|^78411-4646|~151.13|*9.58|^151.13|*9.60|^151.13|*9.60|^151.13|*9.60|^151.13|*9.60|^142.10|*9.77|^142.10|*9.76|^142.10|*9.76|^142.10|*9.76|^142.10|*9.76|^152.71|*9.10|^152.71|*9.10|^152.71|*9.10|^152.71|*9.10|^152.71|*9.10|~1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|^1|*0|~256787116|^256787116|^256787116|^256787116|^256787116|^256787066|^256787066|^256787066|^256787066|^256787066|^256786951|^256786951|^256786951|^256786951|^256786951|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|^001-6628692-501|~56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|^56628692|~CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|^CHARGE OFF|~010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|^010005|~|^|^|^|^|^|^|^|^|^|^|^|^|^|^|~TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|^TX|~01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|^01/10/2016|~03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|^03/31/2022|~0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00";
        }

        public string GetValidSearchInvoiceCsKey()
        {
            return "0|~10 Invoices have been found for contract 201-0090326-160|~10|~1|^2|^3|^4|^5|^6|^7|^8|^9|^10|~4219828690|^4219828691|^4219828692|^4219828924|^4219833239|^4219837779|^4219842677|^4219847574|^4219852205|^4219857549|~17/11/2018|^18/12/2018|^15/01/2019|^15/01/2019|^15/02/2019|^17/03/2019|^17/04/2019|^17/05/2019|^17/06/2019|^18/07/2019|~01/01/2019|^01/02/2019|^01/03/2019|^01/03/2019|^01/04/2019|^01/05/2019|^01/06/2019|^01/07/2019|^01/08/2019|^01/09/2019|~19/07/2019|^19/07/2019|^19/07/2019|^19/07/2019|^19/07/2019|^19/07/2019|^19/07/2019|^19/07/2019|^19/07/2019|^19/07/2019|~829.88|^829.88|^1244.82|^1244.82|^829.88|^829.88|^829.88|^829.88|^829.88|^829.88|~0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^0.00|^829.88|^829.88|~|^|^|^|^|^|^|^|^|^";
        }

        public string GetValidUpdatePromiseToPayDateCsKey()
        {
            return "0|~Promise to pay for invoice 6100072736 updated";
        }

        public string GetValidGetContractCsKey()
        {
            return "0|~1 Contracts have been found.|~1|~201-0090326-160|~239994|~060.000|~|~0083|~DELL EQUIPMENT|~000001.0000|~MY NAME IS: 13*0000010000|~|~|~0|~2783|~MY NAME IS: 10*2783|~MY NAME IS: 201-0090326-160|~|~|~JOHN |~+441582649999|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~CRAWLEY|~|~RH10 9QL|~008|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~CRAWLEY|~WEST SUSSEX|~008|~RH10 9QL|~JOHN Q PUBLIC -T|~1659.76|~829.88|~829.88|~0.00|~0.00|~0.00|~1|~6|~3|~1|~|~0.00|~1|~45|~C|~111111111111|~19/07/2019|~829.88|~01/09/2019|~4219857549|~829.88|~17/08/2019|~0.00|~829.88|~|~|~40002|~01/01/2019|~01/02/2019|~36|~01/01/2022|~|~22406.76|~18257.36|~20963.05|~7|~|~0|~GBP|~01/08/2019|~15/01/2019|~24422.50|~4640.28|~0.00|~TL|~31|~1659.76|~SIN|~|~0007|~11122333|~|~MY NAME IS: 201-0090326-160|~";
        }

        public string GetValidCreateParentCsKey()
        {
            return "1|~700928|~Testing Parent|~Testing Reference";
        }

        public string GetValidSearchDealerCsKey()
        {
            return "1|~10|~0148320000|^0235330000|^0203940000|^0243590000|^0172370000|^0203930000|^0213480000|^0221700000|^0252850000|^0174020000|~MY NAME IS: 13*0148320000|^MY NAME IS: 13*0235330000|^MY NAME IS: 13*0203940000|^MY NAME IS: 13*0243590000|^MY NAME IS: 13*0172370000|^MY NAME IS: 13*0203930000|^MY NAME IS: 13*0213480000|^MY NAME IS: 13*0221700000|^MY NAME IS: 13*0252850000|^MY NAME IS: 13*0174020000|~9999|^0105709999|^13 48 99999|^015222839999|^3459999|^77409999|^56209999|^56209999|^19999|^|~|^JOHN Q PUBL|^JOHN Q PUBLIC -THISISS|^|^|^|^|^|^|^|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~|^|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^|^|^|^|^|^|~CORTEGAÇA OVAR|^GENOVA|^PARIS|^LINCOLNSHIRE|^&apos;S-GRAVENHAGE|^HELSINKI|^TIMISOARA|^TIMISOARA|^MÜNCHEN|^Milano|~|^|^|^|^|^|^|^|^|^MI|~3885-217|^16121|^75016|^LN4 1AZ|^2514HE|^00380|^300174|^300174|^80638|^20145|~PORTUGAL|^ITALY|^FRANCE|^UNITED KINGDOM|^THE NETHERLANDS|^FINLAND|^ROMANIA|^ROMANIA|^GERMANY|^ITALY|~|^|^|^|^3459999|^|^|^|^|^|~DVM|^D|^D|^D|^DVM|^D|^D|^D|^D|^DMV|~|^|^|^|^|^|^|^|^|^|~|^|^|^|^|^|^|^|^|^|~|^|^|^|^|^|^|^|^|^";
        }

        public string GetValidCreateDealerCsKey()
        {
            return "1|~0253400000|~|~Test Dealer 1|~D|~07/05/2022|~Test Address 1|~Test Address 2|~Test City|~Test State|~Test Zip|~Test Country|~Test Contact Name|~Test Contact Phone|~Test Fax|~P|~Test Model|~025340|~0000|~1|~1|~|~";
        }

        public string GetErrorCreateParentCsKey()
        {
            return "99|~+|~PC.Description must not be blank - ABORTING.|~";
        }

        public string GetErrorReserveContractNumberCsKey()
        {
            return "99|~Lessor is null";
        }

        public WebServiceCallMessageResponseDto[] GetValidMessageArray(int ini, int end)
        {
            WebServiceCallMessageResponseDto[] messagearray = new WebServiceCallMessageResponseDto[1 + (end - ini)];
            for (int i = 0; i <= end - ini; i++)
            {
                messagearray[i] = new WebServiceCallMessageResponseDto();
                messagearray[i].pos = (ini + i).ToString();
                messagearray[i].pk = (333222 + i).ToString();
                messagearray[i].MsgKey = (12345000 + i).ToString();
                messagearray[i].maxpos = end.ToString();
                messagearray[i].MsgMasterKey = "201-0090326-160";
                messagearray[i].MsgTime = "15:08:" + (i % 60 >= 10 ? i.ToString() : "0" + i);
                messagearray[i].MsgDate = "2006-10-30";
                messagearray[i].MsgName = "DellTeam";
                messagearray[i].MsgDept = "02";
                messagearray[i].MsgMessage = "Testing a Verified Message: i=" + i.ToString();
                messagearray[i].MsgMessageType = 1;

            }
            return messagearray;
        }

        public List<SearchCustomerResponseCustomerModel> GetValidCustomerListResponse() {
            var CustomersList = new List<SearchCustomerResponseCustomerModel>();
            CustomersList.Add(
                new SearchCustomerResponseCustomerModel()
                {
                    Customer = new SearchCustomerResponseCustomerDataModel()
                    {
                        CCAN = "289187",
                        Name = "MY NAME IS: 017-0128830-001",
                        ShortName = "",
                        DBA = "",
                        Address1 = "",
                        Address2 = "SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED",
                        City = "GENEVE",
                        State = "",
                        Zip = "1205",
                        Country = "041",
                        ContactPhone = "+4111111119999",
                        FederalID = "111223333-TH",
                        Currency = "CHF",
                        Match = "CO",
                        ParentCode = 700045,
                        Score = "3",
                        EffectiveDate = "06/20/2019",
                        Source = "PM",
                        Business = 0,
                        UserCode = "eBoss",
                        Confidentiality = "0"
                    }
                });
            CustomersList.Add(
               new SearchCustomerResponseCustomerModel()
               {
                   Customer = new SearchCustomerResponseCustomerDataModel()
                   {
                       CCAN = "282732",
                       Name = "MY NAME IS: 006-0000123-001",
                       ShortName = "",
                       DBA = "",
                       Address1 = "123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED",
                       Address2 = "",
                       City = "Bucharest",
                       State = "",
                       Zip = "123465",
                       Country = "012",
                       ContactPhone = "132469999",
                       FederalID = "",
                       Currency = "EUR",
                       Match = "CO",
                       ParentCode = null,
                       Score = "3",
                       EffectiveDate = null,
                       Source = "PM",
                       Business = null,
                       UserCode = "__________",
                       Confidentiality = "0"
                   }
               });
            return CustomersList;
        }

        public CreateCustomerResponseModel getValidCreateCustomerResponseModel() {
            return new CreateCustomerResponseModel()
            {
                StatusCode = 0,
                Message = "The record was successfully created.",
                Customer = new CustomerResponseDataModel() {
                    Name = "",
                    DBA = "",
                    ShortName = "",
                    Address1 = "",
                    Address2 = "",
                    Address3 = "",
                    City = "",
                    State = "",
                    Zip = "",
                    Country = "",
                    ARName = "",
                    ARAddress1 = "",
                    ARAddress2 = "",
                    ARAddress3 = "",
                    ARCity = "",
                    ARState = "",
                    ARZip = "",
                    ARCountry = "",
                    ARAttn = "",
                    ContactName = "",
                    ContactPhone = "",
                    ContactFaxPhone = "",
                    Title = "",
                    BankNumber = "",
                    BankAccountNumber = "",
                    Dealer = "",
                    InsuranceAgent = "",
                    FederalID = "",
                    Language = "",
                    SicCode = 0,
                    BusinessType = 0,
                    BirthDate = DateTime.Today,
                    BusinessDesc = "",
                    BeginBusinessYear = 22,
                    RelatedCustomer = "",
                    TaxExemptNumber = "",
                    CreditScoring = "",
                    FacilityScore = "",
                    InvoiceDueDay = 15,
                    ParentCode = 1122334455,
                    ContactEmail = "",
                    Business = 15,
                    BusinessPhone = "",
                    Collector = 2925,
                    MlaNumber = "",
                    MlaDate = "",
                    ObligorScore = 70,
                    CreditBand = "",
                    AlphaNum2 = "",
                    RelatedCodes = 0,
                    ACDCSValue = 0,
                    ACDCSEffDate = DateTime.Today,
                    ACDCSSourceInd = "",
                    ResidentInCountry = true,
                    CustNumEmp = 50,
                    CustTurnover = 40                     
                }
            };
        }

        public webServiceCallResponse GetValidSearchContractsResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr2</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""0|~1 Contracts have been found.|~1|~1|~2002|~|~MY NAME IS: 2002|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~|~Mexico|~007|~11700|~301-0000004-001|~1023|~0008|~023000|~0013|~MY NAME IS: 301-0000004-001|~JOHN Q PUBLIC -THIS|~RL|~36|~02/01/2015|~02/01/2018|~|~USD|~0.00|~|~1|~27|~0102|~|~N|~0|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~Mexico|~007|~MY NAME IS: 301-0000004-001|~CDX|~007|~11700|~|~404|~XAXX010101000|~Various|~0|~0.00|~0.00|~0.00|~0.00|~0.00|~|~|~|~|~0.00|~|~02/01/2018|~0"">
                                <CsKey>0|~1 Contracts have been found.|~1|~1|~2002|~|~MY NAME IS: 2002|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~|~Mexico|~007|~11700|~301-0000004-001|~1023|~0008|~023000|~0013|~MY NAME IS: 301-0000004-001|~JOHN Q PUBLIC -THIS|~RL|~36|~02/01/2015|~02/01/2018|~|~USD|~0.00|~|~1|~27|~0102|~|~N|~0|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~Mexico|~007|~MY NAME IS: 301-0000004-001|~CDX|~007|~11700|~|~404|~XAXX010101000|~Various|~0|~0.00|~0.00|~0.00|~0.00|~0.00|~|~|~|~|~0.00|~|~02/01/2018|~0</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidListParentsResponse()
        {
            return new webServiceCallResponse(@"<Envelope><Header><ApiName>UdaListParent</ApiName><ChLogUserDefinedText /></Header><Body><Response><ParentCode pk=""1884""><CsPcDesc>CITY OF WOLVERHAMPTON COLLEGE </CsPcDesc><CsPcKey>1884</CsPcKey><CsPcReference></CsPcReference></ParentCode><ParentCode pk=""655997""><CsPcDesc>CEMEX GROUP</CsPcDesc><CsPcKey>655997</CsPcKey><CsPcReference></CsPcReference></ParentCode><ParentCode pk=""1670""><CsPcDesc>BONIFICA GROUP</CsPcDesc><CsPcKey>1670</CsPcKey><CsPcReference></CsPcReference></ParentCode></Response></Body></Envelope>");
        }

        public webServiceCallResponse GetValidSearchAssetsResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr2</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidSearchAssetCsKey()}"">
                                <CsKey>{GetValidSearchAssetCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidSearchInvoicesResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr2</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidSearchInvoiceCsKey()}"">
                                <CsKey>{GetValidSearchInvoiceCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidGetContractResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr2</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidGetContractCsKey()}"">
                                <CsKey>{GetValidGetContractCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidUpdatePromiseToPayDateResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr2</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidUpdatePromiseToPayDateCsKey()}"">
                                <CsKey>{GetValidUpdatePromiseToPayDateCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidGetContractsByCCANResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidGetContractsByCCANCsKey()}"">
                                <CsKey>{GetValidGetContractsByCCANCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidCreateParentResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidCreateParentCsKey()}"">
                                <CsKey>{GetValidCreateParentCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidReserveContractNumberResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidReserveContractNumberCsKey()}"">
                                <CsKey>{GetValidReserveContractNumberCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidSearchDealerResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidSearchDealerCsKey()}"">
                                <CsKey>{GetValidSearchDealerCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidCreateDealerResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidCreateDealerCsKey()}"">
                                <CsKey>{GetValidCreateDealerCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidGetContractLinkCodeDetailsByCCANResponse()
        {
            return new webServiceCallResponse(@$"
                <Envelope>
                    <Header>
                        <ApiName>UdaDellCallSubr</ApiName>
                        <ChLogUserDefinedText />
                    </Header>
                    <Body>
                        <Response>
                            <Customer pk=""{GetValidGetContractLinkCodeDetailsByCCANCsKey()}"">
                                <CsKey>{GetValidGetContractLinkCodeDetailsByCCANCsKey()}</CsKey>
                            </Customer>
                        </Response>
                    </Body>
                </Envelope>");
        }

        public webServiceCallResponse GetValidWebServiceCallResponse()
        {
            return new webServiceCallResponse()
            {
                webServiceCallReturn = @"
                    <Envelope>
                        <Body>
                            <webServiceCallReturn>Valid Test</webServiceCallReturn>
                        </Body>
                    </Envelope>"
            };
        }

        public void Dispose()
        {
        }
    }
}
