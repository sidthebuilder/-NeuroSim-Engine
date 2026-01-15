using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {


    public enum GameSpeeds {
        // These values could change with testing and development.
        SPEED_PAUSED = 0,
        SPEED_NORMAL = 1,
        SPEED_FAST   = 2,
        SPEED_FASTER = 4,

    }


    /// <summary>
    /// A store of the in game time in the world.  This is needed
    /// for scaling, among other thing, because scaling Time in the
    /// engine does not effect game logic.  Also, this converts time
    /// to measures meaninful in game.
    /// </summary>
    public sealed class WorldTime {
        private static WorldTime instance;

        // This value could change with testing and development.
        public const double BASE_SCALE = 24;

        // Constants for calculating time units
        public const double PER_MINUTE = 60;
        public const double PER_HOUR   = PER_MINUTE * 60;
        public const double PER_4HOUR  = PER_HOUR * 4;
        public const double PER_DAY    = PER_HOUR * 24;
        public const double PER_WEEK   = PER_DAY * 7;
        public const double PER_MONTH  = PER_WEEK * 4;

        // Data
        private GameSpeeds speed;
        private double gameTime;
        private float  baseDeltaTime;
        private float  deltaTime;
        private double scaling;

        public double GameTime => gameTime;
        public float  BaseDeltaTime => baseDeltaTime;
        public float  DeltaTime => deltaTime;

        public double Minutes => gameTime / PER_MINUTE;
        public double Hours => gameTime / PER_HOUR;
        public double Days => gameTime / PER_DAY;
        public double Weeks => gameTime / PER_WEEK;
        public double Months => gameTime / PER_MONTH;


        public int MinuteOfHour() {
            double hours = Hours;
            return (int)(hours - (int)hours) * 60;
        }

        public int HourOfDay() {
            double days = Days;
            return (int)(days - (int)days) * 24;
        }

        public int DayOfWeek() {
            double weeks = Weeks;
            return (int)(weeks - (int)weeks) * 7;
        }

        public int WeekOfMonth() {
            double months = Months;
            return (int)(months - (int)months) * 4;
        }


        /// <summary>
        /// Set time for both the simulation and the engine (for
        /// stoping things the engine controls directly like
        /// physics and animations).
        /// FIXME?: Not sure this is the best way to do this.
        /// </summary>
        /// <param name="scale"></param>
        public void SetTimeScale(GameSpeeds scale) {
            speed = scale;
            scaling = (double)scale * BASE_SCALE;
            Time.timeScale = (float)scale;
        }


        /// <summary>
        /// Updates the global world time; this should be
        /// called by a managers Update() method.
        /// </summary>
        public void UpdateTime() {
            baseDeltaTime = Time.unscaledDeltaTime;
            deltaTime = baseDeltaTime * (float)scaling;
            gameTime += Time.unscaledDeltaTime * scaling;
        }


        /// <summary>
        /// Replaces the singleton, resetting the global time.
        /// This should only be called for starting a new game.
        /// </summary>
        public void ResetTime() {
            instance = new WorldTime();
        }


        /// <summary>
        /// Sets the game (world) time.  Intended to be used for loading saves.
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(double time) {
            gameTime = time;
        }


        /// <summary>
        /// Allows the world time to be retrieved as a singleton.
        /// There doesn't need to be more than one global world time.
        /// </summary>
        /// <returns></returns>
        public static WorldTime GetWorldTime() {
            if(instance == null) instance = new WorldTime();
            return instance;
        }


        /// <summary>
        /// A more convenient accessor, for situations when it is know the instance shold be known.
        ///
        /// Worldtime should be created when the game starts, this should be known, and this should
        /// never be called out of game.  It is in a sense "unsafe," but properly used should not
        /// be a problem and should avoid excessive conditional branching.
        /// </summary>
        public static WorldTime Instance => instance;
        public static WorldTime t => instance;


        /// <summary>
        /// Privazte constructor to insure the proper use as a singleton.
        /// </summary>
        private WorldTime() {
            gameTime = 0;
            SetTimeScale(GameSpeeds.SPEED_NORMAL);
            UpdateTime();
        }




    }

}
