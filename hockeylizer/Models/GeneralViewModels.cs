using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System;


namespace hockeylizer.Models
{
    public class UploadFileResult
    {
        public UploadFileResult()
        {
            this.FilePath = "";
            this.FileName = "";
        }

        public UploadFileResult(string fp, string fn)
        {
            this.FilePath = fp;
            this.FileName = fn;
        }

        public string FilePath { get; set; }
        public string FileName { get; set; }
    }

    public class UpdateNameVm
    {
        public bool Validate() 
        {
            if (playerId == null)
            {
                this.Result = new GeneralResult(false, "SpelarId är inte med i anropet.");
                return false;
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                this.Result = new GeneralResult(false, "Det nya namnet får inte vara tomt.");
                return false;
            }

            this.Result = new GeneralResult(true, "Allt fixat!");
            return true;
        }

        public int? playerId { get; set; }
        public string name { get; set; }
        public string token { get; set; }

        public GeneralResult Result { get; set; }
    }

    public class DeletePlayerVm
    {
        public bool Validate()
        {
            if (playerId == null)
            {
                this.Result = new GeneralResult(false, "SpelarId är inte med i anropet.");
                return false;
            }

            this.Result = new GeneralResult(true, "Allt fixat!");
            return true;
        }

        public int? playerId { get; set; }
        public string token { get; set; }

        public GeneralResult Result { get; set; }
    }

    public class GeneralResult
    {
        public GeneralResult()
        {
            this.Completed = false;
            this.Description = "Okänt fel";
        }

        public GeneralResult(bool comp, string desc)
        {
            this.Completed = comp;
            this.Description = desc;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
    }

    public class IsAnalyzedResult
    {
        public IsAnalyzedResult()
        {
            this.Completed = false;
            this.Description = "Okänt fel";
            this.Analyzed = false;
        }

        public IsAnalyzedResult(bool comp, string desc, bool al)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Analyzed = al;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public bool Analyzed { get; set; }
    }

    public class GetFramesFromShotResult
	{
		public GetFramesFromShotResult()
		{
			this.Completed = false;
			this.Description = "Okänt fel";
            this.FrameUrls = new List<string>();
		}

		public GetFramesFromShotResult(bool comp, string desc, List<string> urls = null)
		{
			this.Completed = comp;
			this.Description = desc;
            this.FrameUrls = urls == null ? new List<string>() : urls;
		}

		public bool Completed { get; set; }
		public string Description { get; set; }

	    public int? XCoordinate { get; set; }
	    public int? YCoordinate { get; set; }

        public List<string> FrameUrls { get; set; }
	}

	public class GetDataFromShotResult
	{
		public GetDataFromShotResult()
		{
			this.Completed = false;
			this.Description = "Okänt fel";
			this.FrameUrls = new List<string>();
		}

		public GetDataFromShotResult(bool comp, string desc, List<string> urls = null)
		{
			this.Completed = comp;
			this.Description = desc;
			this.FrameUrls = urls == null ? new List<string>() : urls;
		}

		public int TargetNumber { get; set; }
		public int Order { get; set; }

		public int? XCoordinate { get; set; }
		public int? YCoordinate { get; set; }

		public bool Completed { get; set; }
		public string Description { get; set; }
		public List<string> FrameUrls { get; set; }
	}

	public class UpdateTargetHitVm
	{
        public bool Validate()
        {
            // Validation
            if (this.sessionId == null)
            {
                this.ur = new GeneralResult(false, "Sessionsid saknas.");
                return false;
            }

			if (this.shot == null)
			{
				this.ur = new GeneralResult(false, "Skott i skottordningen saknas.");
				return false;
			}

			if (this.x == null)
			{
				this.ur = new GeneralResult(false, "x-koordinat saknas.");
				return false;
			}

			if (this.y == null)
			{
				this.ur = new GeneralResult(false, "y-koordinat saknas.");
				return false;
			}

			this.ur = new GeneralResult(true, "Allt ser korrekt ut.");
			return true;
        }

        public int? sessionId { get; set; }
        public int? shot { get; set; }
        public int? x { get; set; }
        public int? y { get; set; }
        public string token { get; set; }

        public GeneralResult ur { get; set; }
	}

	public class GetTargetFramesVm
	{
		public bool Validate()
		{
			// Validation
			if (this.sessionId == null)
			{
				this.gr = new GetFramesFromShotResult(false, "Sessionsid saknas.");
				return false;
			}

            if (this.shot == null)
            {
                this.gr = new GetFramesFromShotResult(false, "Skott i skottordningen saknas.");
                return false;
            }

			this.gr = new GetFramesFromShotResult(true, "Allt ser korrekt ut.");
			return true;
		}

		public int? sessionId { get; set; }
		public int? shot { get; set; }
		public string token { get; set; }

		public GetFramesFromShotResult gr { get; set; }
	}

    public class SessionVm
    {
        public int sessionId { get; set; }
        public string token { get; set; }
    }

    public class AnalyzeVm
    {
        public int sessionId { get; set; }
        public string filepath { get; set; }
    }

    public class CreateTeamVm
    {
        public string token { get; set; }
    }

    public class AddPlayerVm
    {
        public string name { get; set; }
        public Guid? teamId { get; set; }
        public string token { get; set; }
    }

    public class GetAllPlayersVm
    {
        public Guid? teamId { get; set; }
        public string token { get; set; }
    }

	public class CreateTeamResult
	{
		public CreateTeamResult()
		{
			this.Completed = false;
			this.Description = "Okänt fel";
		}

		public CreateTeamResult(bool comp, string desc)
		{
			this.Completed = comp;
			this.Description = desc;
		}

		public bool Completed { get; set; }
		public string Description { get; set; }

        public Guid? TeamId { get; set; }
	}

    public class  CreateSessionVm
    {
        public bool Validate() 
        {
            if (this.video == null || this.video.Length == 0)
            {
                this.sr = new SessionResult("Videoklippet kunde inte laddas upp då videon saknas!", false);
                return false;
            }

            var allowedFileTypes = "msvideo ogg mp4 avi mpeg quicktime 3gpp 3gpp2";
            var currentFileType = this.video.ContentType.Split('/').LastOrDefault();

            if (!allowedFileTypes.Contains(currentFileType))
            {
                this.sr = new SessionResult("Videoklippet kunde inte laddas upp då videon har fel feltyp! Kan ej ta emot filer av typ: " + currentFileType + ". Endast " + allowedFileTypes + " tas emot.", false);
                return false;
            }

			if (this.interval == null)
			{
				this.sr = new SessionResult("Videoklippet kunde inte laddas upp då intervall saknas!", false);
                return false;
			}

			if (this.rounds == null)
			{
				this.sr = new SessionResult("Videoklippet kunde inte laddas upp då rundor saknas!", false);
                return false;
			}

			if (this.numberOfTargets == null)
			{
				this.sr = new SessionResult("Videoklippet kunde inte laddas upp då antal skott saknas!", false);
                return false;
			}

            if (!this.timestamps.Any())
            {
                this.sr = new SessionResult("Videoklippet kunde inte laddas upp då timestamps saknas!", false);
                return false;
            }

            if (this.timestamps.Any(t => t.start == null || t.end == null))
            {
                this.sr = new SessionResult("Videoklippet kunde inte laddas upp då timestamps saknas!", false);
                return false;
            }

            if (!this.targetOrder.Any())
			{
				this.sr = new SessionResult("Videoklippet kunde inte laddas upp då skottordning saknas!", false);
                return false;
			}


            var to = this.targetOrder.Count;
            var tc = this.targetCoords.Count;
            var ts = this.timestamps.Count;

            if (!(to == tc && to == ts && tc == ts))
            {
                this.sr = new SessionResult("Videoklippet kunde inte laddas upp då antalet listor inte stämmer överens!", false);
                return false;
            }

            this.sr = new SessionResult("Videoklippet laddades upp!", true);
			return true;
        }

        public int? playerId { get; set; }
        public IFormFile video { get; set; }
        public int? interval { get; set; }
        public int? rounds { get; set; }
        public int? shots { get; set; }
        public int? numberOfTargets { get; set; }
        public List<ShotTimestampVm> timestamps { get; set; }
        public List<int> targetOrder { get; set; }
        public List<TargetCoordsVm> targetCoords { get; set; }
        public string token { get; set; }

        public SessionResult sr { get; set; }
    }

    public class ShotTimestampVm
    {
        public int start { get; set; }
        public int end { get; set; }
    }

    public class AddPlayerResult
    {
        public AddPlayerResult()
        {
            this.Description = "Kunde inte lägga till, okänt fel uppstod";
            this.Completed = false;
        }

        public AddPlayerResult(string desc, bool comp, int? pl = null)
        {
            this.PlayerId = pl;
            this.Description = desc;
            this.Completed = comp;
        }

        public int? PlayerId { get; set; }
        public bool Completed { get; set; }
        public string Description { get; set; }
    }

    public class PlayerVmSmall
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }

    public class GetPlayersResult
    {
        public GetPlayersResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Players = new List<PlayerVmSmall>();
        }

        public GetPlayersResult(bool comp, string desc, List<PlayerVmSmall> ps)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Players = ps;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<PlayerVmSmall> Players { get; set; }
    }

    public class SessionVmSmall
    {
        public int SessionId { get; set; }
        public string VideoPath { get; set; }

        public int? Interval { get; set; }
        public int? Rounds { get; set; }
        public int? Shots { get; set; }
        public int? NumberOfTargets { get; set; }

        public List<Target> Targets { get; set; }
    }

    public class GetSessionsResult
    {
        public GetSessionsResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Sessions = new List<SessionVmSmall>();
        }

        public GetSessionsResult(bool comp, string desc, List<SessionVmSmall> ss)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Sessions = ss;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<SessionVmSmall> Sessions { get; set; }
    }

    public class GetFramesResult
    {
        public GetFramesResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Images = new List<string>();
        }

        public GetFramesResult(bool comp, string desc, List<string> imgs)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Images = imgs;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }

        public List<string> Images { get; set; }
    }

    public class SessionResult
    {
        public SessionResult()
        {
            this.Description = "Kunde inte lägga till, okänt fel uppstod";
            this.Completed = false;
        }

        public SessionResult(string desc, bool comp, int? sessionId = null)
        {
            this.Description = desc;
            this.Completed = comp;
            this.SessionId = sessionId;
        }

        public int? SessionId { get; set; }

        public bool Completed { get; set; }
        public string Description { get; set; }
    }

    public class TargetCoordsVm
    {
        public int? xCoord { get; set; }
        public int? yCoord { get; set; }
    }

    //public class DecodeFramesResult
    //{
    //    public DecodeFramesResult() { }

    //    public DecodeFramesResult(int shot, string url)
    //    {
    //        // Constructor
    //        this.Shot = shot;
    //        this.FrameUrl = url;
    //    }

    //    public int Shot { get; set; }
    //    public string FrameUrl { get; set; }
    //}
}
