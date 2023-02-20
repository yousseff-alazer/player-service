using Connect4Sports.Coach.API.CommonDefinitions.Records;
using Player.BL.Services.Managers;
using Player.CommonDefinitions.Requests;
using Player.CommonDefinitions.Responses;
using Reservation.BL.Services;
using Reservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Player.BL.Services
{
	public class LanguageService : BaseService
	{
		#region languageServices
		public static LanguageResponse Listlanguage(LanguageRequest request)
		{
			var res = new LanguageResponse();
			RunBaseMysql(request, res, (LanguageRequest req) =>
			{
				try
				{
					var query = request._context.Languages
					.Where(c => c.IsDeleted != 0).Select(p => new languageRecord
					{

						CreationDate = p.CreationDate,
						Id = p.Id,
						Name = p.Name,
						ModificationDate = p.ModificationDate,
						IconUrl = p.IconUrl,
						Code = p.Code

					});

					if (request.languageRecord != null)
						query = LanguageServiceManager.ApplyFilter(query, request.languageRecord);

					res.TotalCount = query.Count();

					query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);


					query = request.PageSize > 0 ? ApplyPaging(query, request.PageSize, request.PageIndex) : ApplyPaging(query, request.DefaultPageSize, 0);

					res.languageRecords = query.ToList();
					res.Message = HttpStatusCode.OK.ToString();
					res.Success = true;
					res.StatusCode = HttpStatusCode.OK;
				}
				catch (Exception ex)
				{
					res.Message = ex.Message;
					res.Success = false;
					var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
					LogHelper.LogException(request._context, "Languages", "Listlanguage", jsonRequest, ex);

					//cloud Log
					//request._serilogger.Error(ex, ex.Message, "Languages", "Listlanguage");
				}
				return res;
			});
			return res;
		}
		public static LanguageResponse DeleteLanguage(LanguageRequest request)
		{
			var res = new LanguageResponse();
			RunBaseMysql(request, res, req =>
			{
				try
				{
					res.languageRecords = new List<languageRecord>();

					var model = request.languageRecord;
					var language =
						request._context.Languages.FirstOrDefault(c => c.IsDeleted != 0 && c.Id == model.Id);
					if (language != null)
					{
						//update language IsDeleted
						language.IsDeleted = 1;
						language.ModificationDate = DateTime.UtcNow;
						request._context.SaveChanges();
						res.languageRecords.Add(new languageRecord
						{
							CreationDate = language.CreationDate,
							Id = language.Id,
							Name = language.Name,
							ModificationDate = language.ModificationDate,
							IconUrl = language.IconUrl,
							Code = language.Code,
							IsDeleted = language.IsDeleted == 1 ? true:false,
						});
						res.Message = HttpStatusCode.OK.ToString();
						res.Success = true;
						res.StatusCode = HttpStatusCode.OK;
					}
					else
					{
						res.Message = "Invalid language";
						res.Success = false;
					}
				}
				catch (Exception ex)
				{
					res.Message = ex.Message + " " + ex.StackTrace + " " + ex.InnerException;
					res.Success = false;
					var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
					LogHelper.LogException(request._context, "Languages", "DeleteLanguage", jsonRequest, ex);

					//cloud Log
					//request._serilogger.Error(ex, ex.Message, "Languages", "DeleteLanguage");
				}

				return res;
			});
			return res;
		}

		public static LanguageResponse Addlanguage(LanguageRequest request)
		{

			var res = new LanguageResponse();
			RunBaseMysql(request, res, (LanguageRequest req) =>
			{
				try
				{
					res.languageRecords = new List<languageRecord>();
					var language = LanguageServiceManager.AddOrEditlanguage(/*request.CreatedBy,*/ request.languageRecord);
					request._context.Languages.Add(language);
					request._context.SaveChanges();
					res.languageRecords.Add(new languageRecord
					{
						CreationDate = language.CreationDate,
						Id = language.Id,
						Name = language.Name,
						Code = language.Code,
						IconUrl = language.IconUrl,

					});
					res.Message = "AddedSuccessfully";
					res.Success = true;
					res.StatusCode = HttpStatusCode.OK;


				}
				catch (Exception ex)
				{
					res.Message = ex.Message;
					res.Success = false;
					var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
					LogHelper.LogException(request._context, "Languages", "Addlanguage", jsonRequest, ex);

					//cloud Log
					//request._serilogger.Error(ex, ex.Message, "Languages", "AddLanguage");
				}
				return res;
			});
			return res;
		}

		public static LanguageResponse Editlanguage(LanguageRequest request)
		{

			var res = new LanguageResponse();
			RunBaseMysql(request, res, (LanguageRequest req) =>
			{
				try
				{
					res.languageRecords = new List<languageRecord>();
					var model = request.languageRecord;
					var language = request._context.Languages.Find(model.Id);
					if (language != null)
					{
						//update whole Agency
						language = LanguageServiceManager.AddOrEditlanguage(/*request.CreatedBy,*/ request.languageRecord, language);
						request._context.SaveChanges();
						res.languageRecords.Add(new languageRecord
						{
							CreationDate = language.CreationDate,
							Id = language.Id,
							Name = language.Name,
							Code = language.Code,
							IconUrl = language.IconUrl,

						});
						res.Message = "UpdatedSuccessfully";
						res.Success = true;
						res.StatusCode = HttpStatusCode.OK;
					}
					else
					{
						res.Message = "Invalid";
						res.Success = false;
					}
				}
				catch (Exception ex)
				{
					res.Message = ex.Message;
					res.Success = false;
					var jsonRequest = System.Text.Json.JsonSerializer.Serialize(request);
					LogHelper.LogException(request._context, "Languages", "Editlanguage", jsonRequest, ex);

					//cloud Log
					//request._serilogger.Error(ex, ex.Message, "Languages", "EditLanguage");
				}
				return res;
			});
			return res;
		}

		#endregion
	}
}
