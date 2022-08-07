using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Xunit;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Contract;
using Dell.DFS.LLS.IntegrationManagerServices.Api.SharedKernel.IntegrationManagerModels;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Asset;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway.Models.Invoice;

namespace IDSIM.Services.UnitTest.ApiGatewayTests.Models
{
    [ExcludeFromCodeCoverage]
    public class ApiGatewayModelGettersTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public ApiGatewayModelGettersTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void GetContractContractResponseModel_NewInstance_ShouldNotBeNull()
        {
            // Arrange & Act
            var model = new GetContractContractResponseModel()
            {
                ContractNumber = string.Empty,
                CCAN = string.Empty,
                BusinessSegment = string.Empty,
                CreditApplication = string.Empty,
                ProgramType = string.Empty,
                EquipmentDescription = string.Empty,
                DealerNumber = string.Empty,
                DealerName = string.Empty,
                DealerPhone = string.Empty,
                Region = string.Empty,
                MajorAccount = string.Empty,
                MDefCollector = string.Empty,
                DefaultCollector = string.Empty,
                CustomerName = string.Empty,
                CustShortName = string.Empty,
                CustDba = string.Empty,
                ContactName = string.Empty,
                ContactPhone = string.Empty,
                CustomerAddress = new CustomerAddressDataModel()
                {
                    Address1 = string.Empty,
                    Address2 = string.Empty,
                    Address3 = string.Empty,
                    City = string.Empty,
                    State = string.Empty,
                    Zip = string.Empty,
                    Country = string.Empty,
                },
                AccountReceivableAddress = new AccountReceivableAddressDataModel()
                {
                    ARAddress1 = string.Empty,
                    ARAddress2 = string.Empty,
                    ARAddress3 = string.Empty,
                    ARCity = string.Empty,
                    ARState = string.Empty,
                    ARCountry = string.Empty,
                    ARZip = string.Empty,
                    ARAttn = string.Empty,
                    ARName = string.Empty
                },
                DelinquencyDetails = new DelinquencyDetailsDataModel()
                {
                    OutstandingOpenItems = string.Empty,
                    CurrentFutureDue = string.Empty,
                    PastDue1_30 = string.Empty,
                    PastDue31_60 = string.Empty,
                    PastDue61_90 = string.Empty,
                    PastDue91 = string.Empty,
                    DelinStatusCode = string.Empty,
                    TimesDelinquent1_30 = string.Empty,
                    TimesDelinquent31_60 = string.Empty,
                    TimesDelinquent61_90 = string.Empty,
                    TimesDeliquent91 = string.Empty,
                    LateChargesDue = string.Empty,
                },
                AdditionalDetails = new AdditionalDetailsDataModel()
                {
                    NextInvoiceNumber = string.Empty,
                    InvoiceDueDay = string.Empty,
                    LeadInvDay = string.Empty,
                    InvoiceCode = string.Empty,
                    BillingCycle = string.Empty,
                    LastPaymentDate = string.Empty,
                    LastPaymentAmount = string.Empty,
                    LastInvDueDate = string.Empty,
                    LastInvoiceNumber = string.Empty,
                    LastInvoiceAmount = string.Empty,
                    InvoicingDate = string.Empty,
                    MiscAmountIncRent = string.Empty,
                    BaseRentMiscAmount = string.Empty,
                    SalvageValue = string.Empty,
                    ContractStatus = string.Empty,
                    Branch = string.Empty,
                    CommencementDate = string.Empty,
                    FirstPaymentDate = string.Empty,
                    ContractTerm = string.Empty,
                    TermDate = string.Empty,
                    TermDateSecundary = string.Empty,
                    GrossContract = string.Empty,
                    ReceivedBalance = string.Empty,
                    NetInvestiment = string.Empty,
                    NumOfPaymentsInvoiced = string.Empty,
                    SecurityDeposit = string.Empty,
                    AvancedEndingPayments = string.Empty,
                    CurrencyCode = string.Empty,
                    PaidToDate = string.Empty,
                    GlBookingDate = string.Empty,
                    CurrentTotalEquipCost = string.Empty,
                    CurrentResidual = string.Empty,
                    CurrentMgrResidual = string.Empty,
                    ContractType = string.Empty,
                    ArrearStatus = string.Empty,
                    GracePeriod = string.Empty,
                    TotalPastDue = string.Empty,
                    InvoiceFormat = string.Empty,
                    LinkCode = string.Empty,
                    RemitTo = string.Empty,
                    CdFedID = string.Empty
                },
                UserDefFields = new UserDefFieldsDataModel()
                {
                    CuAlphaField9 = string.Empty
                }
            };

            // Assert
            Assert.NotNull(model.ContractNumber);
            Assert.NotNull(model.CCAN);
            Assert.NotNull(model.BusinessSegment);
            Assert.NotNull(model.CreditApplication);
            Assert.NotNull(model.ProgramType);
            Assert.NotNull(model.EquipmentDescription);
            Assert.NotNull(model.DealerNumber);
            Assert.NotNull(model.DealerName);
            Assert.NotNull(model.DealerPhone);
            Assert.NotNull(model.Region);
            Assert.NotNull(model.MajorAccount);
            Assert.NotNull(model.MDefCollector);
            Assert.NotNull(model.DefaultCollector);
            Assert.NotNull(model.CustomerName);
            Assert.NotNull(model.CustShortName);
            Assert.NotNull(model.CustDba);
            Assert.NotNull(model.ContactName);
            Assert.NotNull(model.ContactPhone);

            Assert.NotNull(model.CustomerAddress.Address1);
            Assert.NotNull(model.CustomerAddress.Address2);
            Assert.NotNull(model.CustomerAddress.Address3);
            Assert.NotNull(model.CustomerAddress.City);
            Assert.NotNull(model.CustomerAddress.State);
            Assert.NotNull(model.CustomerAddress.Zip);
            Assert.NotNull(model.CustomerAddress.Country);

            Assert.NotNull(model.AccountReceivableAddress.ARAddress1);
            Assert.NotNull(model.AccountReceivableAddress.ARAddress2);
            Assert.NotNull(model.AccountReceivableAddress.ARAddress3);
            Assert.NotNull(model.AccountReceivableAddress.ARCity);
            Assert.NotNull(model.AccountReceivableAddress.ARState);
            Assert.NotNull(model.AccountReceivableAddress.ARCountry);
            Assert.NotNull(model.AccountReceivableAddress.ARZip);
            Assert.NotNull(model.AccountReceivableAddress.ARAttn);
            Assert.NotNull(model.AccountReceivableAddress.ARName);

            Assert.NotNull(model.DelinquencyDetails.OutstandingOpenItems);
            Assert.NotNull(model.DelinquencyDetails.CurrentFutureDue);
            Assert.NotNull(model.DelinquencyDetails.PastDue1_30);
            Assert.NotNull(model.DelinquencyDetails.PastDue31_60);
            Assert.NotNull(model.DelinquencyDetails.PastDue61_90);
            Assert.NotNull(model.DelinquencyDetails.PastDue91);
            Assert.NotNull(model.DelinquencyDetails.DelinStatusCode);
            Assert.NotNull(model.DelinquencyDetails.TimesDelinquent1_30);
            Assert.NotNull(model.DelinquencyDetails.TimesDelinquent31_60);
            Assert.NotNull(model.DelinquencyDetails.TimesDelinquent61_90);
            Assert.NotNull(model.DelinquencyDetails.TimesDeliquent91);
            Assert.NotNull(model.DelinquencyDetails.LateChargesDue);

            Assert.NotNull(model.AdditionalDetails.NextInvoiceNumber);
            Assert.NotNull(model.AdditionalDetails.InvoiceDueDay);
            Assert.NotNull(model.AdditionalDetails.LeadInvDay);
            Assert.NotNull(model.AdditionalDetails.InvoiceCode);
            Assert.NotNull(model.AdditionalDetails.BillingCycle);
            Assert.NotNull(model.AdditionalDetails.LastPaymentDate);
            Assert.NotNull(model.AdditionalDetails.LastPaymentAmount);
            Assert.NotNull(model.AdditionalDetails.LastInvDueDate);
            Assert.NotNull(model.AdditionalDetails.LastInvoiceNumber);
            Assert.NotNull(model.AdditionalDetails.LastInvoiceAmount);
            Assert.NotNull(model.AdditionalDetails.InvoicingDate);
            Assert.NotNull(model.AdditionalDetails.MiscAmountIncRent);
            Assert.NotNull(model.AdditionalDetails.BaseRentMiscAmount);
            Assert.NotNull(model.AdditionalDetails.SalvageValue);
            Assert.NotNull(model.AdditionalDetails.ContractStatus);
            Assert.NotNull(model.AdditionalDetails.Branch);
            Assert.NotNull(model.AdditionalDetails.CommencementDate);
            Assert.NotNull(model.AdditionalDetails.FirstPaymentDate);
            Assert.NotNull(model.AdditionalDetails.ContractTerm);
            Assert.NotNull(model.AdditionalDetails.TermDate);
            Assert.NotNull(model.AdditionalDetails.TermDateSecundary);
            Assert.NotNull(model.AdditionalDetails.GrossContract);
            Assert.NotNull(model.AdditionalDetails.ReceivedBalance);
            Assert.NotNull(model.AdditionalDetails.NetInvestiment);
            Assert.NotNull(model.AdditionalDetails.NumOfPaymentsInvoiced);
            Assert.NotNull(model.AdditionalDetails.SecurityDeposit);
            Assert.NotNull(model.AdditionalDetails.AvancedEndingPayments);
            Assert.NotNull(model.AdditionalDetails.CurrencyCode);
            Assert.NotNull(model.AdditionalDetails.PaidToDate);
            Assert.NotNull(model.AdditionalDetails.GlBookingDate);
            Assert.NotNull(model.AdditionalDetails.CurrentTotalEquipCost);
            Assert.NotNull(model.AdditionalDetails.CurrentResidual);
            Assert.NotNull(model.AdditionalDetails.CurrentMgrResidual);
            Assert.NotNull(model.AdditionalDetails.ContractType);
            Assert.NotNull(model.AdditionalDetails.ArrearStatus);
            Assert.NotNull(model.AdditionalDetails.GracePeriod);
            Assert.NotNull(model.AdditionalDetails.TotalPastDue);
            Assert.NotNull(model.AdditionalDetails.InvoiceFormat);
            Assert.NotNull(model.AdditionalDetails.LinkCode);
            Assert.NotNull(model.AdditionalDetails.RemitTo);
            Assert.NotNull(model.AdditionalDetails.CdFedID);

            Assert.NotNull(model.UserDefFields.CuAlphaField9);
        }

        [Fact]
        public void GetContractRequestModel_NewInstance_ShouldNotBeNull()
        {
            // Arrange & Act
            var model = new GetContractRequestModel()
            {
                Region = _unitApiTestsFixture.GetValidRegion(),
                ContractNumber = string.Empty
            };

            // Assert
            Assert.NotNull(model.Region);
            Assert.NotNull(model.ContractNumber);
        }

        [Fact]
        public void SearchContractsRequestModel_NewInstance_ShouldNotBeNull()
        {
            // Arrange & Act
            var model = new SearchContractsRequestModel()
            {
                Region = _unitApiTestsFixture.GetValidRegion(),
                ContractNumber = string.Empty,
                CCAN = string.Empty,
                PhoneNumber = string.Empty,
                CreditApplicationNumber = string.Empty,
                SerialTagNumber = string.Empty,
                InvoiceNumber = string.Empty,
                CustomerName = string.Empty,
                ContactName = string.Empty,
                DellOrderNumber = string.Empty,
                FedID = string.Empty,
                PONumber = string.Empty,
                TermDays = string.Empty,
                Paged = string.Empty,
                Pagination = new Pagination()
                {
                    StartIndex = 0,
                    EndIndex = 25,
                    SortField = "TESTE",
                    SortType = "DESC"
                },
            };

            // Assert
            Assert.NotNull(model.Region);
            Assert.NotNull(model.ContractNumber);
            Assert.NotNull(model.CCAN);
            Assert.NotNull(model.PhoneNumber);
            Assert.NotNull(model.CreditApplicationNumber);
            Assert.NotNull(model.SerialTagNumber);
            Assert.NotNull(model.InvoiceNumber);
            Assert.NotNull(model.CustomerName);
            Assert.NotNull(model.ContactName);
            Assert.NotNull(model.DellOrderNumber);
            Assert.NotNull(model.FedID);
            Assert.NotNull(model.PONumber);
            Assert.NotNull(model.TermDays);
            Assert.NotNull(model.Paged);

            Assert.NotNull(model.Pagination.StartIndex);
            Assert.NotNull(model.Pagination.EndIndex);
            Assert.NotNull(model.Pagination.SortField);
            Assert.NotNull(model.Pagination.SortType);
        }

        [Fact]
        public void SearchContractsSearchResultResponseModel_NewInstance_ShouldNotBeNull()
        {
            // Arrange & Act
            var model = new SearchContractsSearchResultResponseModel()
            {
                Position = string.Empty,
                NumberOfFields = string.Empty,
                ContractNumber = string.Empty,
                Branch = string.Empty,
                ProductLine = string.Empty,
                MDefCollector = string.Empty,
                CCAN = string.Empty,
                CustomerName = string.Empty,
                ContactName = string.Empty,
                ARCity = string.Empty,
                ARCountry = string.Empty,
                Phone = string.Empty,
                ContractType = string.Empty,
                CommencementDate = string.Empty,
                TermDatePrimary = string.Empty,
                TermDateSecondary = string.Empty,
                ContractTerm = string.Empty,
                ArrearStatus = string.Empty,
                ARName = string.Empty,
                CBR = string.Empty,
                ContractStatus = string.Empty,
                LastInvDueDate = string.Empty,
                NumberOfAssets = string.Empty,
                CurrentFutureDue = string.Empty,
                ContractPayment = string.Empty,
                BillingCycle = string.Empty,
                CdName = string.Empty,
                CdAddress1 = string.Empty,
                CdAddress2 = string.Empty,
                CdAddress3 = string.Empty,
                CdCity = string.Empty,
                CdCountry = string.Empty,
                CdZip = string.Empty,
                BusinessSegment = string.Empty,
                LCurrencyCode = string.Empty,
                OutstandingOpenItem = string.Empty,
                ProgramType = string.Empty,
                InvDueDay = string.Empty,
                InvDays = string.Empty,
                RemitTo = string.Empty,
                LinkCode = string.Empty,
                InvoiceCode = string.Empty,
                DelinStatusCode = string.Empty,
                ARAddress1 = string.Empty,
                ARAddress2 = string.Empty,
                ARAddress3 = string.Empty,
                ARState = string.Empty,
                ARZip = string.Empty,
                ARAttn = string.Empty,
                DellOrderNum = string.Empty,
                FedID = string.Empty,
                PONumber = string.Empty,
                ReportableDelinqCode = string.Empty,
                LateChargs = string.Empty,
                PastDue_1_30 = string.Empty,
                PastDue_31_60 = string.Empty,
                PastDue_61_90 = string.Empty,
                PastDue_91_Plus = string.Empty,
                TimesDelinq_1_30 = string.Empty,
                TimesDelinq_31_60 = string.Empty,
                TimesDelinq_61_90 = string.Empty,
                TimesDelinq_90_Plus = string.Empty
            };

            // Assert
            Assert.NotNull(model.Position);
            Assert.NotNull(model.NumberOfFields);
            Assert.NotNull(model.ContractNumber);
            Assert.NotNull(model.Branch);
            Assert.NotNull(model.ProductLine);
            Assert.NotNull(model.MDefCollector);
            Assert.NotNull(model.CCAN);
            Assert.NotNull(model.CustomerName);
            Assert.NotNull(model.ContactName);
            Assert.NotNull(model.ARCity);
            Assert.NotNull(model.ARCountry);
            Assert.NotNull(model.Phone);
            Assert.NotNull(model.ContractType);
            Assert.NotNull(model.CommencementDate);
            Assert.NotNull(model.TermDatePrimary);
            Assert.NotNull(model.TermDateSecondary);
            Assert.NotNull(model.ContractTerm);
            Assert.NotNull(model.ArrearStatus);
            Assert.NotNull(model.ARName);
            Assert.NotNull(model.CBR);
            Assert.NotNull(model.ContractStatus);
            Assert.NotNull(model.LastInvDueDate);
            Assert.NotNull(model.NumberOfAssets);
            Assert.NotNull(model.CurrentFutureDue);
            Assert.NotNull(model.ContractPayment);
            Assert.NotNull(model.BillingCycle);
            Assert.NotNull(model.CdName);
            Assert.NotNull(model.CdAddress1);
            Assert.NotNull(model.CdAddress2);
            Assert.NotNull(model.CdAddress3);
            Assert.NotNull(model.CdCity);
            Assert.NotNull(model.CdCountry);
            Assert.NotNull(model.CdZip);
            Assert.NotNull(model.BusinessSegment);
            Assert.NotNull(model.LCurrencyCode);
            Assert.NotNull(model.OutstandingOpenItem);
            Assert.NotNull(model.ProgramType);
            Assert.NotNull(model.InvDueDay);
            Assert.NotNull(model.InvDays);
            Assert.NotNull(model.RemitTo);
            Assert.NotNull(model.LinkCode);
            Assert.NotNull(model.InvoiceCode);
            Assert.NotNull(model.DelinStatusCode);
            Assert.NotNull(model.ARAddress1);
            Assert.NotNull(model.ARAddress2);
            Assert.NotNull(model.ARAddress3);
            Assert.NotNull(model.ARState);
            Assert.NotNull(model.ARCountry);
            Assert.NotNull(model.ARZip);
            Assert.NotNull(model.ARAttn);
            Assert.NotNull(model.DellOrderNum);
            Assert.NotNull(model.FedID);
            Assert.NotNull(model.PONumber);
            Assert.NotNull(model.ReportableDelinqCode);
            Assert.NotNull(model.LateChargs);
            Assert.NotNull(model.PastDue_1_30);
            Assert.NotNull(model.PastDue_31_60);
            Assert.NotNull(model.PastDue_61_90);
            Assert.NotNull(model.PastDue_91_Plus);
            Assert.NotNull(model.TimesDelinq_1_30);
            Assert.NotNull(model.TimesDelinq_31_60);
            Assert.NotNull(model.TimesDelinq_61_90);
            Assert.NotNull(model.TimesDelinq_90_Plus);
        }

        [Fact]
        public void SearchAssetsContractAssetsResponseModel_NewInstance_ShouldNotBeNull()
        {
            // Arrange & Act
            var model = new SearchAssetsContractAssetsResponseModel()
            {
                AssetNumber = string.Empty,
                EquipDescription = string.Empty,
                OpenClosedStatus = string.Empty,
                SerialNumber = string.Empty,
                ModelNumber = string.Empty,
                AssetCost = string.Empty,
                PercentOfTotalAssetCost = string.Empty,
                CurrencyCode = string.Empty,
                ResidualSalvage = string.Empty,
                AssetAddress = string.Empty,
                City = string.Empty,
                Country = string.Empty,
                PostalCode = string.Empty,
                AssetOtherCost = string.Empty,
                AssetOtherFinanced = string.Empty,
                PurchaseOrder = string.Empty,
                EquipmentLocationState = string.Empty,
                DispositionDate = string.Empty,
                TerminationDate = string.Empty,
                Status = string.Empty,
                EquipmentRental = string.Empty,
                Position = string.Empty,
                ContractNo = string.Empty,
                CustCreditAcct = string.Empty,
                BusinessSegment = string.Empty,
                Quantity = string.Empty,
                TableTypeDesc = string.Empty,
                GlTableType = string.Empty,
                GlDispDate = string.Empty,
                Manufacturer = string.Empty,
                AiCtdContra = string.Empty,
                ResidAmt = string.Empty,
                APvAmt = string.Empty,
                ManagersResid = string.Empty,
                PurOption = string.Empty,
                ABranch = string.Empty,
                UserDefFields = new UserDefFieldsModel()
                {
                    AumAlphaNum1 = string.Empty,
                    AumAlphaNum2 = string.Empty,
                    AumUserAmt4 = string.Empty,
                    CauUserAmt1 = string.Empty,
                    CauUserAmt2 = string.Empty,
                }
            };

            // Assert
            Assert.NotNull(model.AssetNumber);
            Assert.NotNull(model.EquipDescription);
            Assert.NotNull(model.OpenClosedStatus);
            Assert.NotNull(model.SerialNumber);
            Assert.NotNull(model.ModelNumber);
            Assert.NotNull(model.AssetCost);
            Assert.NotNull(model.PercentOfTotalAssetCost);
            Assert.NotNull(model.CurrencyCode);
            Assert.NotNull(model.ResidualSalvage);
            Assert.NotNull(model.AssetAddress);
            Assert.NotNull(model.City);
            Assert.NotNull(model.Country);
            Assert.NotNull(model.PostalCode);
            Assert.NotNull(model.AssetOtherCost);
            Assert.NotNull(model.AssetOtherFinanced);
            Assert.NotNull(model.PurchaseOrder);
            Assert.NotNull(model.EquipmentLocationState);
            Assert.NotNull(model.DispositionDate);
            Assert.NotNull(model.TerminationDate);
            Assert.NotNull(model.Status);
            Assert.NotNull(model.EquipmentRental);
            Assert.NotNull(model.Position);
            Assert.NotNull(model.ContractNo);
            Assert.NotNull(model.CustCreditAcct);
            Assert.NotNull(model.BusinessSegment);
            Assert.NotNull(model.Quantity);
            Assert.NotNull(model.TableTypeDesc);
            Assert.NotNull(model.GlTableType);
            Assert.NotNull(model.GlDispDate);
            Assert.NotNull(model.Manufacturer);
            Assert.NotNull(model.AiCtdContra);
            Assert.NotNull(model.ResidAmt);
            Assert.NotNull(model.APvAmt);
            Assert.NotNull(model.ManagersResid);
            Assert.NotNull(model.PurOption);
            Assert.NotNull(model.ABranch);

            Assert.NotNull(model.UserDefFields.AumAlphaNum1);
            Assert.NotNull(model.UserDefFields.AumAlphaNum2);
            Assert.NotNull(model.UserDefFields.AumUserAmt4);
            Assert.NotNull(model.UserDefFields.CauUserAmt1);
            Assert.NotNull(model.UserDefFields.CauUserAmt2);
        }

        [Fact]
        public void SearchInvoicesResponseModel_NewInstance_ShouldNotBeNull()
        {
            // Arrange & Act
            var model = new SearchInvoicesResponseModel()
            {
                Contract = new SearchInvoicesContractResponseModel()
                {
                    ContractNumber = string.Empty,
                    CCAN = string.Empty,
                    CurrencyCode = string.Empty,
                },
                Invoice = new List<SearchInvoicesInvoiceResponseModel>()
                { {
                    new SearchInvoicesInvoiceResponseModel()
                    {
                        Position = string.Empty,
                        InvoiceNumber = string.Empty,
                        InvoiceDate = string.Empty,
                        InvoiceDueDate = string.Empty,
                        InvoiceLastPaymentDate = string.Empty,
                        InvoiceTotal = string.Empty,
                        InvoiceTotalAmountDue = string.Empty,
                        InvoicePromiseToPayDate = string.Empty,
                        InvoiceStatus = string.Empty,
                        Void = string.Empty,
                    }
                }}
            };

            // Assert
            Assert.NotNull(model.Contract.ContractNumber);
            Assert.NotNull(model.Contract.CCAN);
            Assert.NotNull(model.Contract.CurrencyCode);

            Assert.NotNull(model.Invoice[0].Position);
            Assert.NotNull(model.Invoice[0].InvoiceNumber);
            Assert.NotNull(model.Invoice[0].InvoiceDate);
            Assert.NotNull(model.Invoice[0].InvoiceDueDate);
            Assert.NotNull(model.Invoice[0].InvoiceLastPaymentDate);
            Assert.NotNull(model.Invoice[0].InvoiceTotal);
            Assert.NotNull(model.Invoice[0].InvoiceTotalAmountDue);
            Assert.NotNull(model.Invoice[0].InvoicePromiseToPayDate);
            Assert.NotNull(model.Invoice[0].InvoiceStatus);
            Assert.NotNull(model.Invoice[0].Void);
        }
    }
}
