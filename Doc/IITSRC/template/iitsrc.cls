%%
%% This is file `iitsrc.cls',
%% generated with the docstrip utility.
%%
%% The original source files were:
%%
%% iitsrc.dtx  (with options: `iitsrc')
%%
%% IMPORTANT NOTICE:
%%
%% For the copyright see the source file.
%%
%% Any modified versions of this file must be renamed
%% with new filenames distinct from iitsrc.cls.
%%
%% For distribution of the original source see the terms
%% for copying and modification in the file iitsrc.dtx.
%%
%% This generated file may be distributed as long as the
%% original source files, as listed above, are part of the
%% same distribution. (The sources need not necessarily be
%% in the same archive or directory.)
\iffalse meta-comment

    This file is part of the `acmsrc' LaTeX class created for those who
    wish to contribute to IIT.SRC.

    Copyright (C) 2006-2009 Matej Kosik <kosik@fiit.stuba.sk>

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
\fi
%%% From File: iitsrc.dtx
%% Please do not remove the following command:
\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{iitsrc}[2010/12/05]
\LoadClass[12pt,twoside]{article}
\RequirePackage[IL2]{fontenc}
\RequirePackage{cite}
\RequirePackage{times}
\RequirePackage[dvips,dvipdfm,a4paper,centering,textwidth=14cm,top=4.6cm,headsep=.6cm,footnotesep=1cm,footskip=0.6cm,bottom=3.8cm]{geometry}
\RequirePackage{microtype}
\renewcommand{\LARGE}{\@setfontsize\LARGE{20pt}{24pt}}
\renewcommand{\Large}{\@setfontsize\Large{12pt}{14.4pt}}
\renewcommand{\large}{\@setfontsize\large{11pt}{13.2pt}}
\renewcommand{\normalsize}{\@setfontsize\normalsize{10pt}{11.55pt}}
\renewcommand{\small}{\@setfontsize\small{9pt}{10.8pt}}
\renewcommand{\footnotesize}{\@setfontsize\footnotesize{9pt}{10.8pt}}
\renewcommand{\ttdefault}{cmtt}
\DeclareMathSizes{11}{11}{8}{6}
\newcommand\timesrm{\usefont{IL2}{ptm}{m}{n}}
\newcommand\acknowledgementname{Acknowledgement:}
    \gdef\@titlerunning{\@title}
    \newcommand{\titlerunning}[1]{\gdef\@titlerunning{#1}}
    \gdef\@field{UNSPECIFIED FIELD}
    \newcommand{\iip}{Intelligent Information Processing}
    \newcommand{\web}{Web Science and Technologies}
    \newcommand{\se}{Software Engineering}
    \newcommand{\ccs}{Computer Systems, Computer Networks and Security}
    \newcommand{\ai}{Artificial Intelligence}
    \newcommand{\cs}{Computer Science}
    \newcommand{\cg}{Computer Graphics, Image Processing and Multimedia}
    \newcommand{\accrobo}{Accompanying Events --- RoboCup}
    \newcommand{\acccont}{Accompanying Events --- Programming Contest}
    \newcommand{\acctp}{Accompanying Events --- TP CUP}
    \newcommand{\accfoto}{Accompanying Events --- FIITAPIXEL}    
    \newcommand{\field}[1]{\gdef\@field{#1}}
    \renewenvironment{abstract}%
        {%
            \begin{center}%
                \vskip2mm%
                \small
                \begin{minipage}{10cm}%
                    {\bf\abstractname.}%
                    \setlength\parindent{2em}
                    \hskip-.25em
        }%
        {%
                \end{minipage}%
            \end{center}%
            \normalsize
        }%
\renewcommand\labelitemi{--}
\renewcommand\labelitemii{$\circ$}
\renewcommand\labelitemiii{\raise.1em\hbox{\vrule width .3em height .3em}}
\renewcommand\labelenumi{\arabic{enumi}.}
\renewcommand\labelenumii{\alph{enumii}.}
\renewcommand\labelenumiii{\roman{enumiii}.}
\newcommand\acknowledgement[1]{%
    \vskip1em%
    \noindent{\em\acknowledgementname}\hskip\labelsep#1%
}
\newcommand\editpages[2]{%
    \setcounter{page}{#1}%
    \gdef\@startpage{#1}%
    \gdef\@endpage{#2}%
}
\gdef\@startpage{1}
\gdef\@endpage{1}
\gdef\@bottomnote{%
    \ifnum\@startpage=\@endpage
        IIT.SRC 2015, Bratislava, April 28, 2016, p.~\@startpage
    \else
        IIT.SRC 2015, Bratislava, April 28, 2016, pp.~\@startpage--\@endpage
    \fi
}
\gdef\ps@empty{%
    \let\@mkboth\@gobbletwo
    \let\@oddhead\@empty
    \def\@oddfoot{\hfill\footnotesize\it\@bottomnote.}%
    \let\@evenhead\@empty
    \def\@evenfoot{\hfill\footnotesize\it\@bottomnote.}%
}
\gdef\ps@myheadings{%
    \let\@oddfoot\@empty
    \let\@evenfoot\@empty
    \def\@evenhead{\small\it\hbox to 0.8cm{\thepage\hfil}\leftmark\hfil}
    \def\@oddhead{\small\it\hfil\rightmark\hbox to 0.8cm{\hfil\thepage}}
}
\renewcommand\@maketitle{%
    \begin{center}%
        \vbox{}%
        \vskip1.65cm%
        \begin{minipage}{\hsize}%
            \begin{center}%
                \LARGE\@title
            \end{center}%
        \end{minipage}%
        \vskip.9cm%
        \ifnum\arabic{@authorlinenumber}=0%
            \unvbox\@authorlines
            \hbox to \hsize{%
                \hfill\unhbox\@authorline\hfill
            }%
        \else
            \unvbox\@authorlines
            \vskip\@authorlineskip
            \hbox to \hsize{%
                \hfill\unhbox\@authorline\hfill
            }%
        \fi
        \vskip3mm%
        {\it Slovak University of Technology in Bratislava\\
        Faculty of Informatics and Information Technologies\\
        Ilkovi\v cova 2, 842 16 Bratislava, Slovakia}\\
        {\tt\@mail}%
    \end{center}%
}
\renewcommand\maketitle{\par
    \begingroup
        \renewcommand\thefootnote{%
            \@fnsymbol\c@footnote
        }%
        \def\@makefnmark{%
            \rlap{\@textsuperscript{\normalfont\@thefnmark}}%
        }%
        \long\def\@makefntext##1{%
            \noindent
            \hskip1em%
            \vbox{%
                % The following `advance' is necessary due to the last `hskip'
                % command which moves the whole vbox 1em to the right.
                \advance\hsize-1em%
                \hskip-1em\@textsuperscript{\normalfont\@thefnmark}%
                \hskip.5em%
                ##1%
            }%
        }%
        \newpage
        \global\@topnum\z@
        \@maketitle
        \thispagestyle{empty}%
        \@thanks
    \endgroup
    \setcounter{footnote}{0}%
    \global\let\thanks\relax
    \global\let\maketitle\relax
    \global\let\@maketitle\relax
    \global\let\@thanks\@empty
    \global\let\@author\@empty
    \global\let\@date\@empty
    \global\let\@title\@empty
    \global\let\title\relax
    \global\let\author\relax
    \global\let\date\relax
    \global\let\and\relax
}
\long\def\@makecaption#1#2{%
    \vskip\abovecaptionskip
    \sbox\@tempboxa{\small{\em #1. #2}}%
    \ifdim \wd\@tempboxa >\hsize
        \center{\small{\em #1. #2}}\par
    \else
        \global\@minipagefalse
        \hb@xt@\hsize{\hfil\box\@tempboxa\hfil}%
    \fi
    \vskip\belowcaptionskip
}
\newbox\@authorlines
\setbox\@authorlines=\vbox{}
\newbox\@authorline
\setbox\@authorline=\hbox{}
\newcounter{@authorinline}
\setcounter{@authorinline}0
\newcounter{@authorlinenumber}
\setcounter{@authorlinenumber}0
\newcommand\@authorlineskip{.2em}
\gdef\@firstauthorname{}
\newcounter{@numberofauthors}
\setcounter{@numberofauthors}0
\renewcommand\author[2]{%
    \renewcommand\thefootnote{\@fnsymbol\c@footnote}%
    \stepcounter{@authorinline}%
    \stepcounter{@numberofauthors}%
    \ifnum\arabic{@numberofauthors}=1%
        \gdef\@firstauthorname{#1~#2}%
    \fi
    \ifnum\arabic{@authorinline}=1%
        % This is the first author in the line.
        \setbox\@authorline=\hbox{{\timesrm#1}~{\sc#2}}%
    \else
        \ifnum\arabic{@authorinline}=4%
            % This would be the fourth author in the line.
            \ifnum\arabic{@authorlinenumber}=1%
                % This is the first line of authors.
                \setbox\@authorlines=\vbox{%
                    \unvbox\@authorlines
                    \hbox to \hsize{\hfill\unhbox\@authorline\hfill}%
                }%
            \else
                % This is not the first line of authors.
                \setbox\@authorlines=\vbox{%
                    \unvbox\@authorlines
                    \vskip\@authorlineskip
                    \hbox to \hsize{\hfill\unhbox\@authorline\hfill}%
                }%
            \fi
            \setbox\@authorline=\hbox{{\timesrm#1}~{\sc#2}}%
            \setcounter{@authorinline}1%
            \stepcounter{@authorlinenumber}%
        \else
            % This is the second or the third author in the line
            \setbox\@authorline=\hbox{%
                \unhbox\@authorline, {\timesrm#1}~{\sc#2}%
            }%
        \fi
    \fi
}
\newcommand\supervision[4]
    {%
        \setbox\@authorline\hbox{%
            \unhbox\@authorline
            \thanks{#1 study programme in field: #2\\Supervisor: #3, #4}}%
    }
\newcommand\bc{Bachelor}
\newcommand\ms{Master}
\newcommand\phd{Doctoral}
\newcommand\info{Informatics}
\newcommand\compeng{Computer Engineering}
\newcommand\softeng{Software Engineering}
\newcommand\infosys{Information Systems}
\newcommand\appinf{Applied Informatics}
\newcommand\fiit{Faculty of Informatics and Information Technologies STU in Bratislava}
\newcommand\iai{Institute of Applied Informatics}
\newcommand\iise{Institute of Informatics and Software Engineering}
\newcommand\icsn{Institute of Computer Systems and Networks}
    \long\def\@makefntext#1{%
        \noindent
        \hskip1em%
        \vbox{%
            \hskip-1em\@textsuperscript{\normalfont\@thefnmark}%
            \hskip.5em%
            #1%
        }%
    }%
\gdef\@mail{}
\newcommand\mail[1]{\gdef\@mail{#1}}
\AtBeginDocument{%
    \ifnum\arabic{@numberofauthors}=1%
        % there is only one author
        \markboth{\@field}%
                 {\@firstauthorname:~\@titlerunning}%
    \else
        % there are more than one authors
        \markboth{\@field}%
                 {\@firstauthorname~et~al.:~\@titlerunning}%
    \fi
    \maketitle
    \pagestyle{myheadings}%
    \renewcommand\thefootnote{\arabic{footnote}}%
}
\endinput
%%
%% End of file `iitsrc.cls'.
