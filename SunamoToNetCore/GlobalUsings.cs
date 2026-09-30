global using System.Collections.Generic;
global using System;
global using System.Linq;
global using System.Text;
global using System.Collections;
global using System.IO;
global using System.Threading.Tasks;
global using System.Diagnostics;
global using System.Text.RegularExpressions;
global using System.Xml.Linq;
global using System.Reflection;
global using System.Net;
global using System.Runtime.CompilerServices;
global using System.Xml;
global using System.Diagnostics.CodeAnalysis;
global using System.Runtime.Versioning;
global using System.Web;
global using Case.NET.Extensions;
global using HtmlAgilityPack;
global using Diacritics.Extensions;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using ILogger = Microsoft.Extensions.Logging.ILogger;
global using NullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger;
global using SunamoToNetCore.ToNetCore.Results;
global using TextCopy;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase;
global using SunamoToNetCore._sunamo.SunamoDevCodeCore;
global using SunamoToNetCore._sunamo.SunamoSolutionsIndexer;
global using SunamoToNetCore.research;
global using SunamoToNetCore.ToNetCore.Consts;
global using SunamoToNetCore._sunamo.SunamoAps.Aps;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase.Enums;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase.Values;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._public;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo;
global using SunamoToNetCore._sunamo.SunamoDevCodeCore.Interfaces;
global using SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Algorithms;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Enums;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Helpers;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Projs;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._public.SunamoCollectionsNonGeneric;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._public.SunamoGitBashBuilder;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._public.SunamoTextOutputGenerator;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoBts;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoCollections;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoCollectionsChangeContent;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoCollectionsGeneric;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoEnumsHelper;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoExceptions;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoExtensions;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoFileExtensions;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoFileSystem;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoGetFiles;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoRegex;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoResult;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoString;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringGetLines;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringParts;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringReplace;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringSplit;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoStringTrim;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoTextOutputGenerator;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoThisApp;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoTwoWayDictionary;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoValues;
global using SunamoToNetCore._sunamo.SunamoMsBuild.MsBuild.Values;
global using SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer.Args;
global using SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer.Enums;
global using SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer.Interfaces;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Projs.Enums;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Projs.Results;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._public.SunamoData.Data;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._public.SunamoEnums.Enums;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoInterfaces.Interfaces;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoLang.SunamoI18N;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoLang.SunamoXlf;
global using SunamoToNetCore._sunamo.SunamoSolutionsIndexer.SunamoSolutionsIndexer.Data.SolutionFolderNs;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Projs.Data.ItemGroup;
global using SunamoToNetCore._sunamo.SunamoAps.Aps.Interfaces;
global using SunamoToNetCore._sunamo.SunamoDevCodeBase._sunamo.SunamoPlatformUwpInterop;
