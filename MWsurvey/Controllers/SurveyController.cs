using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MWsurvey.Models;
using System.Text.RegularExpressions;

namespace MWsurvey.Controllers
{
    public class SurveyController : Controller
    {
        private MWsurveyEntities db = new MWsurveyEntities();

        public ActionResult Patient()
        {
            List<SurveyBasic> basic = db.SurveyBasic.ToList();
            var patient = from b in basic
                          where b.SurveyBasicSn == 1
                          select b;
            return View(patient);
        }

        public ActionResult Carer()
        {
            List<SurveyBasic> basic = db.SurveyBasic.ToList();
            var carer = from b in basic
                        where b.SurveyBasicSn == 2
                        select b;
            return View(carer);
        }

        public ActionResult Medicalworker()
        {
            List<SurveyBasic> basic = db.SurveyBasic.ToList();
            var medicalWorker = from b in basic
                                where b.SurveyBasicSn == 3
                                select b;
            return View(medicalWorker);
        }

        public ActionResult MedicalEngineer()
        {
            List<SurveyBasic> basic = db.SurveyBasic.ToList();
            var medicalEngineer = from b in basic
                                  where b.SurveyBasicSn == 4
                                  select b;
            return View(medicalEngineer);
        }

        public ActionResult Delivery()
        {
            List<SurveyBasic> basic = db.SurveyBasic.ToList();
            var delivery = from b in basic
                           where b.SurveyBasicSn == 5
                           select b;
            return View(delivery);
        }

        public ActionResult Cleaner()
        {
            List<SurveyBasic> basic = db.SurveyBasic.ToList();
            var cleaner = from b in basic
                          where b.SurveyBasicSn == 6
                          select b;
            return View(cleaner);
        }


        public ActionResult Index(int surNum)
        {
            Session["Url"] = surNum;
            List<SurveyBasic> basic = db.SurveyBasic.ToList();
            List<SurveyGroup> gRoup = db.SurveyGroup.ToList();
            List<SurveyQuestion> question = db.SurveyQuestion.ToList();
            List<SurveyOption> option = db.SurveyOption.ToList();
            List<SurveyRecord> record = db.SurveyRecord.ToList();
            var survey = from b in basic
                         where b.SurveyBasicSn == surNum
                         join g in gRoup on b.SurveyBasicSn equals g.SurveyBasicSn
                         into table1
                         from g in table1.ToList()
                         join q in question on g.SurveyGroupSn equals q.SurveyGroupSn
                         into table2
                         from q in table2.ToList()
                         join o in option on q.SurveyQuestionSn equals o.SurveyQuestionSn
                         into table3
                         from o in table3.ToList()
                         select new V_SURVEY_LIST
                         {
                             basic = b,
                             gRoup = g,
                             question = q,
                             option = o
                         };
            Console.WriteLine(survey);
            return View(survey);
        }

        [HttpPost]
        public JsonResult CheckValidPerson(FormCollection data)
        {
            string result = "Null";
            string session = data[0];
            if (session != "")
            {
                if (session != null)
                {
                    Session["ID"] = session;
                    result = "Success";
                }
                result = "Fail";
            }

            return Json(result);
        }

        public ActionResult SaveResponseData(FormCollection formdata)
        {
            SurveyRecordRoot surveyRecordRoot = new SurveyRecordRoot()
            {
                UserType = "0" + Session["Url"].ToString(),
                UserID = Session["ID"].ToString(),
                InsTime = DateTime.Now
            };
            db.SurveyRecordRoot.Add(surveyRecordRoot);
            db.SaveChanges();

            var rootSnLine = db.SurveyRecordRoot.OrderByDescending(x => x.SRootSn).FirstOrDefault();
            var sRootSn = rootSnLine.SRootSn;

            int n = formdata.Count;
            int i = 0;
            foreach (var data in formdata)
            {
                if (formdata[i] != "")
                {
                    if (formdata.AllKeys[i].Contains("option"))
                    {
                        int qNum = int.Parse(Regex.Replace(formdata.AllKeys[i], "[^0-9]", ""));
                        SurveyRecord surveyRecord = new SurveyRecord()
                        {
                            SurveyQuestionSn = qNum,
                            OptionValue = formdata[i],
                            SRootSn = sRootSn,
                        };
                        db.SurveyRecord.Add(surveyRecord);
                        db.SaveChanges();
                    }
                    if (formdata.AllKeys[i].Contains("radiotext"))
                    {
                        int qNum = int.Parse(Regex.Replace(formdata.AllKeys[i], "[^0-9]", ""));
                        //string num = Session["ID"].ToString();
                        var DataItem = db.SurveyRecord.OrderByDescending(x => x.SurveyRecordSn).FirstOrDefault(x => x.SurveyQuestionSn == qNum);
                        DataItem.TxtValue = formdata[i];
                        db.SaveChanges();
                    }
                    if (formdata.AllKeys[i].Contains("box"))
                    {
                        int qNum = int.Parse(Regex.Replace(formdata.AllKeys[i], "[^0-9]", ""));
                        SurveyRecord surveyRecordText = new SurveyRecord()
                        {
                            //EmployeeNum = Session["ID"].ToString(),
                            SurveyQuestionSn = qNum,
                            TxtValue = formdata[i],
                            SRootSn = sRootSn,
                        };
                        db.SurveyRecord.Add(surveyRecordText);
                        db.SaveChanges();
                    }

                }
                i++;
            }

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
