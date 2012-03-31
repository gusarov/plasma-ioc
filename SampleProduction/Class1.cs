using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TurboFac;

namespace SampleProduction
{
	public class Movie
	{
		public string Name { get; set; }
	}

	public interface IMovieFinder
	{
		IEnumerable<Movie> GetAll();
	}

	[TurboReg]
	public class MovieFinder : IMovieFinder
	{
		public IEnumerable<Movie> GetAll()
		{
			yield return new Movie { Name = "test1" };
			yield return new Movie { Name = "test2" };
			yield return new Movie { Name = "tst" };
			yield return new Movie { Name = "tmp" };
		}
	}

	[TurboReg]
	public class MovieLister
	{
		readonly IMovieFinder _movieFinder;

		public MovieLister(IMovieFinder movieFinder)
		{
			_movieFinder = movieFinder;
		}

		public IEnumerable<Movie> Find(string substr)
		{
			return _movieFinder.GetAll().Where(x => x.Name.Contains(substr));
		}
	}
}
