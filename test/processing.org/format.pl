#
# This script could be used once example-batch-importer ran through the
# entire examples. Usage:
#	find . -name *.html > index.txt
#	perl format.pl < index.txt > index.html
# Then upload the entire output directory.
@line = <STDIN>;
print <<END;
<html>
<head>
<title>tsukimi test: imported processing.org examples</title>
</head>
<body>
<h1>tsukimi test: imported from processing.org examples</h1>

<ul>
END

foreach $link (@line) {
	print "<li><a href=\"$link\">$link</a></li>";
}

print <<END;
</ul>
</body>
</html>
END

