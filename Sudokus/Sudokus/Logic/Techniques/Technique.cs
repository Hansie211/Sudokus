using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudokus.Logic.Techniques {

    public abstract class Technique {

        public SudokuSolver Solver { get; }

        public Technique( SudokuSolver solver ) {

            Solver = solver;
        }

        public static IEnumerable<Technique> GetTechniques( SudokuSolver solver ) {

            //var techniqueTypes = typeof( Technique ).Assembly.GetTypes().Where( type => type.IsSubclassOf( typeof( Technique ) ) && !type.IsAbstract );
            //foreach( var techniqueType in techniqueTypes ) {

            //    yield return (Technique)Activator.CreateInstance( techniqueType, solver );
            //}

            // yield return new NakedSubsetTechnique( solver );
            yield return new SingleOptionTechnique( solver );
            yield return new TwoDirectionTechnique( solver );
        }

        public abstract void ReduceOptions();
    }
}
