﻿using Microsoft.AspNetCore.Http;
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

    public class ValidateEmailVm
    {
        public string token { get; set; }
        public string email { get; set; }
    }

    public class EmailVm
    {
        public string token { get; set; }
        public int? sessionId { get; set; }
        public string email { get; set; }
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

    public class IsChoppedResult
    {
        public IsChoppedResult()
        {
            this.Completed = false;
            this.Description = "Okänt fel";
            this.Chopped = false;
        }

        public IsChoppedResult(bool comp, string desc, bool ch)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Chopped = ch;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public bool Chopped { get; set; }
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

	    public double? XCoordinate { get; set; }
	    public double? YCoordinate { get; set; }

        public List<string> FrameUrls { get; set; }
	}

    public class GetDataFromSessionResult
    {
        public GetDataFromSessionResult()
        {
            this.Completed = false;
            this.Description = "Okänt fel";
            this.Analyzed = false;
        }

        public GetDataFromSessionResult(bool comp, string desc)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Analyzed = false;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }

        public bool Analyzed { get; set; }
        public string HitRatio { get; set; }
    }

    public class GetDataFromShotResult
	{
		public GetDataFromShotResult()
		{
			this.Completed = false;
			this.Description = "Okänt fel";
            this.HitTarget = false;
            this.Analyzed = false;
			this.FrameUrls = new List<string>();
		}

		public GetDataFromShotResult(bool comp, string desc, List<string> urls = null)
		{
			this.Completed = comp;
			this.Description = desc;
            this.HitTarget = false;
            this.Analyzed = false;
			this.FrameUrls = urls ?? new List<string>();
		}

		public int TargetNumber { get; set; }
		public int Order { get; set; }

        public double? XOffset{ get; set; }
        public double? YOffset { get; set; }

        public double? XCoordinate { get; set; }
		public double? YCoordinate { get; set; }

	    public double? XCoordinateAnalyzed { get; set; }
	    public double? YCoordinateAnalyzed { get; set; }

        public bool Completed { get; set; }
		public string Description { get; set; }

        public bool? HitTarget { get; set; }
        public int? FrameHit { get; set; }

        public bool Analyzed { get; set; }
        public string Reason { get; set; }

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
        public double? x { get; set; }
        public double? y { get; set; }
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

    // Daniels försök
    public class GetSvgVm
    {
        public bool Validate()
        {
            // Validation
            if (this.sessionId == null)
            {
                this.description = "Sessionsid saknas.";
                return false;
            }
            if (string.IsNullOrEmpty(token))
            {
                this.description = "Token saknas.";
                return false;
            }
            if (string.IsNullOrEmpty(returnType) || string.IsNullOrWhiteSpace(returnType))
            {
                this.description = "returnType saknas ('link' eller 'svg').";
                return false;
            }

            this.description = "Allt ser korrekt ut.";
            return true;
        }

        public int? sessionId { get; set; }
        public string returnType { get; set; }
        public string token { get; set; }

        public string description { get; set; }
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

    public class CreateSessionVm
    {
        public bool Validate() 
        {
            if (this.video == null || this.video.Length == 0)
            {
                this.sr = new SessionResult("Videoklippet kunde inte laddas upp då videon saknas!", false);
                return false;
            }

            const string allowedFileTypes = "mp4 avi mpeg mov";
            string currentFileType;

            var firstAlt = this.video.ContentType.Split('/').LastOrDefault();

            if (allowedFileTypes.Contains(firstAlt))
            {
                currentFileType = firstAlt;
            }
            else if (this.video.ContentType.Contains("quicktime"))
            {
                currentFileType = "mov";
            }
            else
            {
				this.sr = new SessionResult("Videoklippet kunde inte laddas upp då videon har fel videotyp! Endast " + allowedFileTypes + " tas emot. Error 1. " + this.video.ContentType, false);
				return false;
            }

            if (!allowedFileTypes.Contains(currentFileType))
            {
                this.sr = new SessionResult("Videoklippet kunde inte laddas upp då videon har fel videotyp! Kan ej ta emot filer av typ: " + currentFileType + ". Endast " + allowedFileTypes + " tas emot.", false);
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

            if (this.targetCoords.Count() != 5)
			{
				this.sr = new SessionResult("Videoklippet kunde inte laddas upp då antalet koordinater att sikta på inte är 5!", false);
				return false;
			}

            if (this.targetCoords.Any(tc => tc.xCoord == null) || this.targetCoords.Any(tc => tc.yCoord == null))
			{
				this.sr = new SessionResult("Videoklippet kunde inte laddas upp någon av koordinaterna att sikta på var null!", false);
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
