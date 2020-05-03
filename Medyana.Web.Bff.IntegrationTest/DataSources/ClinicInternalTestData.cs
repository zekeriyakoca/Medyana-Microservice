﻿using Dtos.Common;
using Dtos.Enums;
using Medyana.Dtos.Clinic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medyana.Web.Bff.IntegrationTest.DataSources
{
  public class ClinicInternalTestData
  {
    public static IEnumerable<object[]> ClinicsToInsert
    {
      get
      {
        yield return new object[] { new ClinicInsertDto { Name = "Test Clinic 1" }, true };
        yield return new object[] { new ClinicInsertDto { Name = "123" }, true };
      }
    }
    public static IEnumerable<object[]> GetPaginatedClinicsReqeusts
    {
      get
      {
        yield return new object[] { new PaginationRequestDto { Column = "name", IsAscending = true, Page = 0, PageItemCount = 5, SearchText = "" }, true };
        yield return new object[] { new PaginationRequestDto { Column = "name", IsAscending = false, Page = 0, PageItemCount = 5, SearchText = "" }, true };
        yield return new object[] { new PaginationRequestDto { Column = "name", IsAscending = true, Page = 0, PageItemCount = 5, SearchText = "random name jskdfksdf" }, false };
        yield return new object[] { new PaginationRequestDto { Column = "", IsAscending = true, Page = 0, PageItemCount = 5, SearchText = "" }, true };

        //Todo : implement...

      }
    }

    

  }
}
