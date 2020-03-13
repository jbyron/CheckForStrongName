using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace CheckForSignature {
    class Program {
        static void Main( string[] args ) {
            bool fileFound = false;

            if (args.Length < 1) {
                writeUsage();
                return;
            }

            string path = args[0];
            if (!Directory.Exists( path )) {
                Console.WriteLine( "Specified path not found" );
                return;
            }

            Console.WriteLine( string.Format( "Scanning path: {0}", path ) );

            string[] files = Directory.GetFiles( path );

            foreach (string filename in files) {
                string fullPath = Path.Combine( path, filename );
                string ext = Path.GetExtension( fullPath );

                if (ext == ".dll" | ext == ".exe") {
                    fileFound = true;
                    Console.WriteLine( "{0} : {1}", Path.GetFileName(filename).PadLeft( 60 ), (isFileSigned( fullPath ) ? "signed" : "unsigned").PadRight( 10 ) );
                }
            }

            if (!fileFound) {
                Console.WriteLine( "Specified path does not contain any DLL or EXE files" );
                return;
            }

            if(System.Diagnostics.Debugger.IsAttached) {
                Console.Write( "Press Any Key to Exit..." );
                Console.ReadKey();
            }

            return;
        }



        private static void writeUsage() {
            Console.WriteLine( "Usage:" );
            Console.WriteLine( "CheckForStrongName.exe folder-path" );
            Console.WriteLine( "Example: CheckForStrongName c:\\temp\\folder\\" );
        }

        private static bool isFileSigned( string fullPath ) {
            bool rv = true;

            try {

                Assembly asm = Assembly.LoadFrom( fullPath );
                rv = asm.GetName().GetPublicKey().Length > 0;

            } catch (Exception ex) {
                throw;
            }

            return rv;
        }
    }
}
