using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Dell.DFS.LLS.IntegrationManagerServices.Api.ApiGateway;

namespace IDSIM.Services.UnitTest.ApiGatewayTests
{
    [ExcludeFromCodeCoverage]
    public class CsKeySerializerTests : IClassFixture<UnitApiTestsFixture>
    {
        private readonly UnitApiTestsFixture _unitApiTestsFixture;

        public CsKeySerializerTests(UnitApiTestsFixture unitApiTestsFixture)
        {
            _unitApiTestsFixture = unitApiTestsFixture;
        }

        [Fact]
        public void SearchContractsRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidSearchContractsRequestModel();

            var actualFormat = $"DELL.SEARCH.CONTRACTS" +
                $"|~{model.Region}" +
                $"|~{model.ContractNumber}" +
                $"|~{model.CCAN}" +
                $"|~{model.PhoneNumber}" +
                $"|~{model.CreditApplicationNumber}" +
                $"|~{model.SerialTagNumber}" +
                $"|~{model.InvoiceNumber}" +
                $"|~{model.CustomerName}" +
                $"|~{model.ContactName}" +
                $"|~{model.DellOrderNumber}" +
                $"|~{model.FedID}" +
                $"|~{model.PONumber}" +
                $"|~{model.TermDays}" +
                $"|~{model.Paged}" +
                $"|~{model.Pagination?.StartIndex}" +
                $"|~{model.Pagination?.EndIndex}" +
                $"|~{model.Pagination?.SortField}" +
                $"|~{model.Pagination?.SortType}";

            // Act
            var response = CsKeySerializer.SearchContractsRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void SearchContractsResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = "0|~4 Contracts have been found.|~4|~1|^2|^3|^4|~2002|^2002|^2002|^2002|~|^|^|^|~MY NAME IS: 2002|^MY NAME IS: 2002|^MY NAME IS: 2002|^MY NAME IS: 2002|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~Ciudad de México|^Ciudad de México|^Ciudad de México|^Ciudad de México|~007|^007|^007|^007|~11700|^11700|^11700|^11700|~101-0000004-001|^101-0000004-002|^101-0000004-003|^301-0000004-001|~1023|^1023|^1023|^1023|~0001|^0001|^0001|^0008|~023000|^023000|^023000|^023000|~0164|^0164|^0164|^0164|~MY NAME IS: 101-0000004-001|^MY NAME IS: 101-0000004-002|^MY NAME IS: 101-0000004-003|^MY NAME IS: 301-0000004-001|~JOHN Q PUBLIC -|^JOHN Q PUBLIC |^JOHN Q PUBLIC |^JOHN Q PUBLIC -THIS|~TL|^TL|^TL|^RL|~36|^36|^24|^36|~09/01/2020|^11/01/2020|^11/01/2020|^02/01/2015|~09/01/2023|^11/01/2023|^11/01/2022|^02/01/2018|~|^|^|^|~USD|^USD|^USD|^USD|~3422.36|^689.13|^750.11|^0.00|~|^|^|^|~1|^1|^1|^1|~27|^27|^27|^27|~0102|^0102|^0102|^0102|~|^|^|^|~C|^C|^C|^N|~0|^0|^0|^0|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~|^|^|^|~Mexico|^Mexico|^Mexico|^Mexico|~007|^007|^007|^007|~MY NAME IS: 101-0000004-001|^MY NAME IS: 101-0000004-002|^MY NAME IS: 101-0000004-003|^MY NAME IS: 301-0000004-001|~CDX|^CDX|^CDX|^CDX|~007|^007|^007|^007|~11700|^11700|^11700|^11700|~|^|^|^|~T12560|^T12935|^T13056|^404|~111223333-TH|^111223333-TH|^111223333-TH|^111223333-TH|~Various|^Various|^Various|^Various|~0|^0|^0|^0|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~|^|^|^|~|^|^|^|~|^|^|^|~|^|^|^|~73757.75|^16040.16|^9699.75|^0.00|~|^|^|^|~09/01/2021|^09/01/2021|^09/01/2021|^02/01/2018|~182|^18|^21|^0";

            // Act
            var response = CsKeySerializer.SearchContractsResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void SearchContractsResponseFromString_MoreThan61Indexes_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = "0|~4 Contracts have been found.|~4|~1|^2|^3|^4|~2002|^2002|^2002|^2002|~|^|^|^|~MY NAME IS: 2002|^MY NAME IS: 2002|^MY NAME IS: 2002|^MY NAME IS: 2002|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^ATTN: JANE F COMMON -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~Ciudad de México|^Ciudad de México|^Ciudad de México|^Ciudad de México|~007|^007|^007|^007|~11700|^11700|^11700|^11700|~101-0000004-001|^101-0000004-002|^101-0000004-003|^301-0000004-001|~1023|^1023|^1023|^1023|~0001|^0001|^0001|^0008|~023000|^023000|^023000|^023000|~0164|^0164|^0164|^0164|~MY NAME IS: 101-0000004-001|^MY NAME IS: 101-0000004-002|^MY NAME IS: 101-0000004-003|^MY NAME IS: 301-0000004-001|~JOHN Q PUBLIC -|^JOHN Q PUBLIC |^JOHN Q PUBLIC |^JOHN Q PUBLIC -THIS|~TL|^TL|^TL|^RL|~36|^36|^24|^36|~09/01/2020|^11/01/2020|^11/01/2020|^02/01/2015|~09/01/2023|^11/01/2023|^11/01/2022|^02/01/2018|~|^|^|^|~USD|^USD|^USD|^USD|~3422.36|^689.13|^750.11|^0.00|~|^|^|^|~1|^1|^1|^1|~27|^27|^27|^27|~0102|^0102|^0102|^0102|~|^|^|^|~C|^C|^C|^N|~0|^0|^0|^0|~123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^123 LOCATION DRIVE -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|^SUITE 5000 -THISISSCRAMBLEDDATATHATCANNOTBEREVERSED|~|^|^|^|~Mexico|^Mexico|^Mexico|^Mexico|~007|^007|^007|^007|~MY NAME IS: 101-0000004-001|^MY NAME IS: 101-0000004-002|^MY NAME IS: 101-0000004-003|^MY NAME IS: 301-0000004-001|~CDX|^CDX|^CDX|^CDX|~007|^007|^007|^007|~11700|^11700|^11700|^11700|~|^|^|^|~T12560|^T12935|^T13056|^404|~111223333-TH|^111223333-TH|^111223333-TH|^111223333-TH|~Various|^Various|^Various|^Various|~0|^0|^0|^0|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~0.00|^0.00|^0.00|^0.00|~|^|^|^|~|^|^|^|~|^|^|^|~|^|^|^|~73757.75|^16040.16|^9699.75|^0.00|~|^|^|^|~09/01/2021|^09/01/2021|^09/01/2021|^02/01/2018|~182|^18|^21|^0|~teste1|^teste2|^teste3|^teste4|~teste11|^teste22|^teste33|^teste44";

            // Act
            var response = CsKeySerializer.SearchContractsResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void SearchContractsResponseFromString_InvalidCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.SearchContractsResponseFromString(cskey));
        }

        [Fact]
        public void SearchAssetsRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidSearchAssetsRequestModel();

            var actualFormat = $"DELL.SEARCH.ASSETS" +
                $"|~{model.Region}" +
                $"|~{model.CCAN}" +
                $"|~{model.ContractNumber}" +
                $"|~{model.SerialNumber}" +
                $"|~{(model.Pagination == null ? "N" : "Y")}" +
                $"|~{model.Pagination?.StartIndex?.ToString()}" +
                $"|~{model.Pagination?.EndIndex?.ToString()}" +
                $"|~{model.Pagination?.SortType}" +
                $"|~{model.TermBeginDate}" +
                $"|~{model.TermEndDate}" +
                $"|~{model.EquipmentZipCode?.ToString()}" +
                $"|~{model.DellOrderNumber}";

            // Act
            var response = CsKeySerializer.SearchAssetsRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void SearchAssetsResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidSearchAssetCsKey();

            // Act
            var response = CsKeySerializer.SearchAssetsResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void SearchAssetsResponseFromString_InvalidCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.SearchAssetsResponseFromString(cskey));
        }

        [Fact]
        public void SearchInvoicesRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidSearchInvoicesRequestModel();

            var actualFormat = $"DELL.SEARCH.INVOICES" +
                   $"|~{model.Region?.ToString()}" +
                   $"|~{model.ContractNumber}" +
                   $"|~{model.InvoiceNumber}" +
                   $"|~{(!string.IsNullOrEmpty(model.Category) && !string.IsNullOrWhiteSpace(model.Category) ? model.Category : "All")}" +
                   $"|~{(model.Pagination == null ? "N" : "Y")}" +
                   $"|~{model.Pagination?.StartIndex?.ToString()}" +
                   $"|~{model.Pagination?.EndIndex?.ToString()}" +
                   $"|~{model.Pagination?.SortField}" +
                   $"|~{model.Pagination?.SortType}";

            // Act
            var response = CsKeySerializer.SearchInvoicesRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void SearchInvoicesResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidSearchInvoiceCsKey();
            var contractDetails = _unitApiTestsFixture.GetValidGetContractResponseModel();

            // Act
            var response = CsKeySerializer.SearchInvoicesResponseFromString(cskey, contractDetails);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void SearchInvoicesResponseFromString_InvalidCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;
            var contractDetails = _unitApiTestsFixture.GetValidGetContractResponseModel();

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.SearchInvoicesResponseFromString(cskey, contractDetails));
        }

        [Fact]
        public void GetContractRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidGetContractRequestModel();

            var actualFormat = $"DELL.GET.CONTRACT" +
                   $"|~{model.Region?.ToString()}" +
                   $"|~{model.ContractNumber}";

            // Act
            var response = CsKeySerializer.GetContractRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void GetContractResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidGetContractCsKey();

            // Act
            var response = CsKeySerializer.GetContractResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void GetContractResponseFromString_InvalidCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.GetContractResponseFromString(cskey));
        }

        [Fact]
        public void UpdatePromiseToPayDateToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidUpdatePromiseToPayDateRequestModel();

            var splittedDate = model.PromiseToPayDate.Split("-");
            var day = splittedDate[2];
            var month = splittedDate[1];
            var year = splittedDate[0];

            var actualFormat = $"DELL.UPDATE.PROMISE.TO.PAY" +
            $"|~{model.Region?.ToString()}" +
            $"|~{model.SfdcPortalUsername}" +
            $"|~{model.CCAN}" +
            $"|~{model.ContractNumber}" +
            $"|~{model.InvoiceNumber}" +
            $"|~{month}/{day}/{year}";

            // Act
            var response = CsKeySerializer.UpdatePromiseToPayDateToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void UpdatePromiseToPayDateResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidUpdatePromiseToPayDateCsKey();

            // Act
            var response = CsKeySerializer.UpdatePromiseToPayDateResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void UpdatePromiseToPayDateResponseFromString_InvalidCsKey_ShouldReturnError()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.UpdatePromiseToPayDateResponseFromString(cskey));
        }

        [Fact]
        public void GetContractsByCCANToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidApiGatewayGetContractsByCCANRequestModel();

            var actualFormat = $"NF.GET.CCAN.CONTRACT.LIST" +
            $"|~{model.CCAN}" +
            $"|~{(model.SearchType == "Middle" ? "0" : "1")}";

            // Act
            var response = CsKeySerializer.GetContractsByCCANToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void GetContractsByCCANResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidGetContractsByCCANCsKey();

            // Act
            var response = CsKeySerializer.GetContractsByCCANResponseFromString(cskey);

            // Assert
            Assert.Equal(1, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void GetContractsByCCANResponseFromString_EmptyCsKey_ShouldReturnNoContract()
        {
            // Arrange
            var cskey = string.Empty;

            // Act
            var response = CsKeySerializer.GetContractsByCCANResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
            Assert.Equal($"no active contracts available", response.Message);
        }

        [Fact]
        public void CreateParentRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidCreateParentRequestModel();

            var actualFormat = $"NF.CREATE.PARENT" +
            $"|~{model.Description}" +
            $"|~{model.Reference}";

            // Act
            var response = CsKeySerializer.CreateParentRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void CreateParentResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidCreateParentCsKey();

            // Act
            var response = CsKeySerializer.CreateParentResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void CreateParentResponseFromString_EmptyCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.CreateParentResponseFromString(cskey));
        }

        [Fact]
        public void ReserveContractNumberRequestToString_ValidRequestModelWithMiddle_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidReserveContractNumberRequestModel();

            var actualFormat = $"NF.RESERVE.CONTRACT.SCHEDULE" +
            $"|~{model.Lessor}" +
            $"|~{model.Middle}";

            // Act
            var response = CsKeySerializer.ReserveContractNumberRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void ReserveContractNumberRequestToString_ValidRequestModelWithoutMiddle_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidReserveContractNumberRequestModel(withMiddle: false);

            var actualFormat = $"NF.RESERVE.CONTRACT" +
            $"|~{model.Lessor}" +
            $"|~";

            // Act
            var response = CsKeySerializer.ReserveContractNumberRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void ReserveContractNumberResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidReserveContractNumberCsKey();

            // Act
            var response = CsKeySerializer.ReserveContractNumberResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void ReserveContractNumberResponseFromString_EmptyCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.ReserveContractNumberResponseFromString(cskey));
        }

        [Fact]
        public void SearchDealerRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidSearchDealerRequestModel();

            var actualFormat = $"NF.GET.DEALER" +
            $"|~{model.SearchType}" +
            $"|~{model.Name}" +
            $"|~{model.Type}" +
            $"|~{model.ContactPhone}";

            // Act
            var response = CsKeySerializer.SearchDealerRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void SearchDealerResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidSearchDealerCsKey();

            // Act
            var response = CsKeySerializer.SearchDealerResponseFromString(cskey);

            // Assert
            Assert.Equal(1, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void SearchDealerResponseFromString_EmptyCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.SearchDealerResponseFromString(cskey));
        }

        [Fact]
        public void CreateDealerRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidCreateDealerRequestModel();

            var actualFormat = $"NF.CREATE.DEALER|~|~" +
                $"|~{model.Name}" +
                $"|~{model.Type}|~" +
                $"|~{model.Address1}" +
                $"|~{model.Address2}" +
                $"|~{model.City}" +
                $"|~{model.State}" +
                $"|~{model.Zip}" +
                $"|~{model.Country}" +
                $"|~{model.ContactName}" +
                $"|~{model.ContactPhone}" +
                $"|~{model.Fax}" +
                $"|~{model.PaymentPlan}" +
                $"|~{model.Model}" +
                $"|~{model.Prefix}" +
                $"|~{model.Suffix}" +
                $"|~{model.PrefixStart?.ToString().Replace(".", "")}" +
                $"|~{model.SuffixStart?.ToString().Replace(".", "")}|~|~";

            // Act
            var response = CsKeySerializer.CreateDealerRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void CreateDealerResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidCreateDealerCsKey();

            // Act
            var response = CsKeySerializer.CreateDealerResponseFromString(cskey);

            // Assert
            Assert.Equal(0, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void CreateDealerResponseFromString_EmptyCsKey_ShouldReturnError()
        {
            // Arrange
            var cskey = string.Empty;

            // Act
            var response = CsKeySerializer.CreateDealerResponseFromString(cskey);

            // Assert
            Assert.Equal(-1, response.StatusCode);
            Assert.Equal("Internal error while creating dealer", response.Message);
        }

        [Fact]
        public void GetContractLinkCodeDetailsByCCANRequestToString_ValidRequestModel_ShouldFormatString()
        {
            // Arrange
            var model = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANRequestModel();

            var actualFormat = $"NF.CUST.LINK.CODE|~{model.CCAN}";

            // Act
            var response = CsKeySerializer.GetContractLinkCodeDetailsByCCANRequestToString(model);

            // Assert
            Assert.Equal(response, actualFormat);
        }

        [Fact]
        public void GetContractLinkCodeDetailsByCCANResponseFromString_ValidCsKey_ShouldDeserializeSuccessfully()
        {
            // Arrange
            var cskey = _unitApiTestsFixture.GetValidGetContractLinkCodeDetailsByCCANCsKey();

            // Act
            var response = CsKeySerializer.GetContractLinkCodeDetailsByCCANResponseFromString(cskey);

            // Assert
            Assert.Equal(1, response.StatusCode);
            Assert.NotNull(response);
        }

        [Fact]
        public void GetContractLinkCodeDetailsByCCANResponseFromString_EmptyCsKey_ShouldThrowException()
        {
            // Arrange
            var cskey = string.Empty;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => CsKeySerializer.GetContractLinkCodeDetailsByCCANResponseFromString(cskey));
        }
    }
}
