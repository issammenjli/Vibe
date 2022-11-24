# <p align="center"> ASP.Net Core 6 Les fondamentaux </p>


Le présent article est un cas pratique d’un mini-projet développé en ASP. Net Core 6. Le but est de survoler les grandes lignes du .Net Core afin de faciliter l’assimilation des différents socles techniques. Pour cela nous allons implémenter une application Vibe permettant de lister les chanteurs les plus célèbres😉. Par la suite nous allons enrichir la solution tout en décortiquant les points les plus important. 
Premièrement, le code source est disponible sur Git.
Le projet sera composé de trois couches front (UI), back (WebAPI) et Entities pour la couche transverse.

### 1/Front

#### Démarrage et hébergement d'une application
Le .Net Core nous permet de créer des applications front rendues par le serveur ou par le browser. 
Ainsi on distingue deux modèles rendus par le serveur
•	Modèle basé sur des pages Razor 
•	Modèle basé sur le MVC 

Et deux modèles rendus par le browser
•	Blazor WebAssembly 
•	Blazor Server 

Voici les étapes à suivre pour créer le projet :
 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/1.png)

Choisir le nom du projet « Vibe » 
![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/2.png)
Nous allons travailler avec .Net 6.0 
![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/3.png)

Cliquer sur « créer ».

Pour notre exemple j'utilise un modèle basé sur des pages Razor.
Comment cela fonctionne : Malgré cette diversité de modèles, chaque application ASP.NET Core commence sa vie comme une application console. Lorsque l'application est démarrée, le code dans le Program.cs est exécuté. Tout d'abord, un objet Builder est créé puis utilisé pour obtenir un objet App, qui est finalement commandé pour être lancé.  Une fois lancée, l’application console se transforme en une application ASP.NET Core. 
Lorsqu’on clique sur le bouton Executer/Run dans Visual Studio, ce qui lancera une session de débogage (réalisable également en ligne de commande dotnet run), le navigateur s'ouvre et affiche la page Web par défaut. Mais il y a aussi une autre application qui se lance et qui ressemble à une application console. Alors à quoi ça sert ? 
Cette application effectue certaines journalisations et reste à l’écoute sur l'URL utilisé par l’application. Cela signifie donc que notre application Web contient son propre serveur Web et que les navigateurs peuvent lui parler directement. Ce serveur Web intégré s'appelle Kestrel.

NB : Lors de l'exécution d'une application en production, Kestrel ne suffit souvent pas. Dans ce cas, il est possible de mettre un reverse proxy devant votre application comme IIS sous Windows. Ce serveur Web utilisera ensuite l'outil .NET CLI pour exécuter l'application et la maintenir en cours d'exécution. Les demandes entrantes vers IIS seront ensuite transmises à Kestrel, et les réponses passeront de Kestrel à IIS. 
Les développeurs peuvent imiter ce comportement au moment du développement en choisissant IIS Express dans la liste déroulante, puis en exécutant l'application normalement. 

"... Kestrel isn’t a full-featured web server; rather, it’s a small, fast web server geared toward serving dynamic content from ASP.NET Core. 
The recommended way to use Kestrel in a production environment is to place it behind a reverse proxy. 
The reverse proxy can handle things that Kestrel isn’t well suited for—like serving static files, SSL termination, and response compression." 
-- Microservices in .NET
-- Christian Horsdal Gammelgaard

Nous abordons maintenant un point très axial dans la conception et le développent des applications ASP.NET Core qui est l’injection de dépendance !



#### Injection de dépendance
![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/program.cs.PNG)

Revenons au fichier Program.cs. La partie juste avant la ligne qui crée l'objet app enregistre les services dans le conteneur d'injection de dépendance. 

// Add services to the container.
builder.Services.AddRazorPages();

L'injection de dépendances est une partie essentielle de chaque application ASP.NET Core. Le conteneur est un endroit central dans lequel les types peuvent être enregistrés. 
Chaque instance de type aura une durée de vie, qui est déterminée au moment de l'enregistrement. Une fois l'enregistrement est effectué, nous pouvons demander une instance d'objet n'importe où dans l'application. L'objet sera automatiquement instancié lorsque nous le demanderons pour la première fois.

Maintenant comment ça fonctionne :

Builder.Services est l'objet sur lequel s’effectue l'enregistrement avant le lancement de l’application. AddRazorPages est méthode d'extension qui enregistre tous les types nécessaires pour le moteur Razor Pages. 
Vous pouvez également enregistrer vos propres types dans un conteneur d'injection de dépendances. 

#### Middleware et pipeline de requêtes
Là aussi on parle d’une notion très importante. Dans le fichier Program.cs, entre la création de l’objet app et son exécution, il y a une succession d’appels de méthode sur l'objet app, préfixe généralement par le mot Use. 

C’est là où nous configurons le pipeline de requêtes internes et c'est la partie où nous spécifions les fonctionnalités ASP.NET Core voulues. Une fois qu'une demande atteint notre application via Kestrel, elle est traitée par un certain nombre d'étapes qui constituent le pipeline de la demande. 
Ces étapes sont appelées middleware. Tous les Middlewares dans l'ordre auront la possibilité d’interagir avec la requête, formant la réponse. Si vous ne configurez aucun middleware dans votre application, cette dernière ne fera tout simplement rien lorsqu'elle recevra une requête. 

Dans notre cas, nous devons brancher un middleware pour le routage, qui mappe l'URL utilisé dans le navigateur à une partie de notre application. Il existe aussi un middleware pour rendre disponibles des fichiers statiques, tels que des images et des fichiers CSS et JavaScript. Il en va de même pour l'autorisation. Ainsi, dans le code, vous trouverez UseRouting, UseStaticFiles et UseAuthorization, qui sont toutes des méthodes d'extension qui configurent le middleware dans le pipeline. 
On aussi le UseRouting qui active une fonctionnalité de routage générale, et le MapRazorPages qui s'assurera que l'URL est bien mappée à une page Razor correspondante. 

L'ordre dans lequel le middleware est branché est important puisque les données de la demande parcourent le middleware dans l'ordre. Avant d'afficher une page Razor vous effectuez par exemple une autorisation. 
Il y a autre chose à dire sur le middleware c’est le StaticFiles. Seuls les fichiers présents dans le dossier wwwroot sont accessibles par ce middleware, donc tous les fichiers statiques doivent s'y trouver. 
Vous pouvez trouver d’autres exemples de construction de Middlewares, et il est également possible de créer le vôtre. 

#### Pages Razor : pages, routage et helpers
Comment écrire la partie interface utilisateur d'une application ASP.NET Core ?
Avant de répondre je devrais introduire certaines notions.
Le mécanisme de routage d'ASP.NET Core prend l'URL reçu et la mappe à une Razor Page.
Dans cet exemple, le BaseURL/index est interprété comme une requête pour une page nommée index. Le serveur fera tout ce qui est nécessaire pour afficher la page et rendre le code HTML au navigateur.
Il y a quelque chose de spécial à propos de la page index, c'est une page par défaut donc avec ou sans le /index, derrière l'URL, cela affichera toujours la même page.

![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/index.PNG)

Revenons à nos moutons 😊. La première ligne déclare une page Razor. Le modèle utilisé dans Razor est une liste de chanteurs et à chaque itération un objet Singer est traité.
Maintenant, d'où vient le modèle ? 
Lorsque je clique sur le petit triangle devant Index.cshtml, Index.cshtml.cs apparaît  et qui emballe la classe IndexModel.
 
 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/IndexModel.png)
 
La classe IndexModel découle de la classe de base PageModel. Elle a une propriété Singers référencée dans Index.cshtml. Elle a aussi une méthode OnGet() qui charge un Singers en appelant la méthode GetAll(). Et le GetAll est appelé sur l’objet SingerRepository injecté par le conteneur d'injection.

La méthode OnGet() est appelée un handler method. Elle sera exécutée lorsque la page recevra une requête get.

Retour à la page Index, l’autre chose intéressante dans le Razor est la balise spéciale asp‑page, ce n'est pas un attribut HTML standard mais un exemple de helper. Cet attribut est traité par ASP.NET Core lorsqu'il restitue la page. 

Asp‑page prend le nom d'une page et le transforme en une URL, qui est ensuite rendue dans l’attribut href. Cela générera donc un lien vers la page Create.

#### Pages Razor : publication de données, validation et liaison de données

Jetons un œil sur la page Create.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Create.PNG)
 
Elle contient un formulaire HTML qui n'y a rien de spécial à l'exception du fait que des helpres sont utilisés :

Asp‑validation‑summary affiche une liste de toutes les erreurs de validation dès qu'elles sont présentes.
Asp‑for, présent à la fois dans le lable et l’input, référence une propriété dans le modèle NewSinger.
 
  ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/CreateModel.PNG)
  
Voici le CreateModel avec la propriété NewSinger. Il est décoré avec un attribut BindProperty. Cela signifie que dès que le formulaire est posté, cet objet sera automatiquement rempli avec les valeurs postées. C'est ce qu'on appelle la liaison de données binding.
Alors comment cela fonctionne :
Les éléments d'entrée avec les assistants de balise asp-for dans le Razor ressembleront à ceci lorsque le code HTML sera envoyé au navigateur.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Post.PNG)
 
Nos deux inputs NewSinger.Name et NewSinger.Nickname ont deux des valeurs.
Lorsque le formulaire est publié, ASP.NET Core essaie désormais de lier les valeurs à un objet avec un attribut BindProperty dont le nom est spécifié dans le nom de l'entrée avant le point.
Dans ce cas, nous avons une correspondance. Par la suite, il recherchera la propriété spécifiée après le point. Étant donné que la classe Singer a une propriété Name et Nickname, les valeurs de propriété dans l'objet obtiendront automatiquement leurs valeurs à partir des données publiées.
Revenant à la classe Model, elle a également une méthode handler, mais cette fois, la méthode sera déclenchée lorsqu'une demande de publication est reçue lors de la publication du formulaire.
 
 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/onCreate.PNG)
 
Dans celui-ci, nous disons simplement au SingerRepository d'ajouter un nouveau Singer.
ModelState IsValid vérifiera s'il y a des erreurs de validation.
Les attributs d'annotation de données seront utilisés pour valider les données entrantes.
 
 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Entities.png)
 
Lorsqu'il y a une erreur de validation, CAD le ModelState est invalide, nous renvoyons un objet qui provient d'un appel à la méthode Page(). Il s'agit d'un objet qui implémente l'interface IActionResult.
Cet objet s'assurera que la page actuelle, dans ce cas, la page Create, est réaffichée, mais puisqu'il y a maintenant des erreurs de validation, le résumé de validation s'affichera.
Lorsqu'il n'y a pas d'erreurs de validation, après l'ajout du Singer au repository, un objet RedirectToPage est renvoyé et qui implémente également IActionResult. 
Cela redirigera vers la page spécifiée, dans ce cas, l'index, qui affichera à nouveau la liste des Singers.

### 2/ Backend
De même que la partie front, on dispose de pas mal d’options pour implémenter la partie back : WebAPI, gRPC et même le SignalR peut être considéré comme une API qui prend en charge la communication serveur -> client.

#### Applications API
Une API est une interface logicielle qui permet d’interagir avec nos services afin déchanger des données. Donc une API doit pouvoir envoyer et recevoir des données. Nous utilisons la sérialisation pour cela en transformant des objets C# dans un format, comme le JSON, qui peut être envoyé à travers le réseau.
L'extrémité réceptrice peut alors restituer les données sérialisées, à nouveau, sous forme d'objets en utilisant la désérialisation.

Voici la nouvelle architecture de l'application qui inclut les API.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Architecture.png)
 
Le navigateur interagira avec le serveur pour obtenir les pages, comme nous l'avons vu.
Lorsque des données sont nécessaires, l'application serveur contactera l'API pour obtenir les données.
Lorsque de nouvelles données sont introduites ou modifiées, elle contacte à nouveau l'API pour les faire persister.
 
#### REST

L'API Web dans ASP.NET Core nous permet d'écrire des API basées sur REST, également appelées API RESTful.

Généralement, dans une API REST, le protocole HTTP est exploité. Nous utilisons des requêtes et des réponses HTTP, et chaque élément de données est disponible sur un point de terminaison unique appelé End point.
Une liste de chanteurs, par exemple, pourrait être disponible sur /Singers et un chanteur avec un ID de 1 sur /Singer/1.
Nous utilisons les méthodes HTTP, telles que GET, POST et PUT, pour déterminer ce que nous voulons faire.

Un GET est utilisé pour récupérer des données et un POST pour introduire des nouvelles données. Avec un PUT, nous mettons à jour les données.
Lorsqu'une réponse revient, nous pouvons examiner le code d'état de la requête HTTP pour voir comment cela s'est passé. Par exemple un code d'état HTTP 200 lors d'un get indiquera que tout s'est bien passé, et les données sont contenues dans la réponse. Un code d'état 404 indiquerait qu'il n'a pas pu trouver les données demandées.


Les réponses peuvent également contenir des pointeurs sous la forme d'URL avec des suggestions sur ce qu'il faut faire ensuite. Lorsqu'un nouveau Singer est introduit avec un POST, la réponse peut contenir l'URL sur laquelle trouver le nouveau Singer. 
return CreatedAtAction(nameof(GetOne), new { id = Singer.Id }, Singer);
L'API Web peut prendre en charge d'autres formats, mais dans la grande majorité des cas JSON sera utilisé comme format de sérialisation par défaut.

#### Web API

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Solution.PNG)
 
Rappelez-vous que nous avons trois projets dans la solution, une application frontale Vibe.UI (Razor pages), Vibe.WebApi (le projet Web API) et Vibe.Entities (une bibliothèque de classes qui partage la classe de Singer entre les différentes couches).
 
Expliquons, note API Web.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Capture%20API.PNG)
 
Dans Program.cs, nous ajoutons la prise en charge du Controller au conteneur d'injection de dépendances builder.Services.AddControllers(). 
Vers la fin, on mappe les Controllers, app.MapControllers(), mais cette fois sans table de routage.
Alors, comment l'API Web sait-elle comment mapper les URL aux actions du Controller ?  
Avant de répondre, je tiens à souligner deux autres choses dans Program.cs.
Tout d'abord, la prise en charge de Swagger est ajoutée app.UseSwagger(). Swagger ou le OpenAPI est une norme qui décrit une API REST.
Nous ajoutons également la prise en charge d’interface utilisateur Swagger, app.UseSwaggerUI(),  pour accéder à la documentation web de cette API et voir sur quels points de terminaisons cette API est disponible.
Revenant au Program.cs, la prise en charge de CORS est également ajoutée. app.UseCors(...);
Les requêtes effectuées par des applications qui s'exécutent dans le navigateur en dehors du domaine utilisé par l'application frontale ne sont pas possibles, sauf avec une autorisation explicite. Avec CORS, je peux autoriser explicitement notre application Razor page.

Voici le cœur de l'API, le Controller.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Controller.PNG)
 
C'est une classe qui dérive de ControllerBase. C’est essentiellement un Controller sans prise en charge de la vue. Il est décoré par l'attribut ApiController. Cet attribut active les fonctionnalités de l'API sur le Controller. L'une des choses qu'il fait est d'exiger un routage d'attribut au lieu d'une table de routage.
Nous spécifions maintenant la route de base pour le Controller comme ceci [Route("[controller]")]. 
Controller ici est une expression qui évaluera le nom du Controller sans le suffixe donc, dans ce cas, Singer.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/GetAllController.PNG)
 
Le Controller a une action appelée GetAll, qui renvoie un ActionResult. Cette fois, nous utilisons ActionResults qui transmet un code d'état HTTP en réponse afin que les appelants puissent voir le résultat de leur demande. Lorsqu'il n'y a pas de Singers, nous renvoyons NoContent. C'est le code d'état HTTP 204, par exemple. Et lorsque tout a été réussi, nous retournons le code de statut Ok 200 avec la liste des Singers. Quant à la sérialisation de l'objet Singer, elle se fait automatique.
L'action GetAll est configurée pour répondre à une requête GET. Cela signifie que lorsqu'une requête GET est envoyée à /Singer, cette action s'exécutera.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/GetOneController.PNG)
 
GetOne répond également à une requête GET, mais il dispose d'informations de routage supplémentaires. Il est configuré pour attendre un ID dans l'URL qui doit être un nombre entier. S'il y a une correspondance avec cette route, l'action se déclenchera, et le paramètre id obtiendra automatiquement sa valeur à partir de l'URL. Nous obtenons le Singer à partir de la repository à l'aide de l'ID. S'il n'y a pas de Singer de ce type, nous renvoyons le code d'état HTTP 404 NotFound. S'il y en a, nous retournons à nouveau Ok avec le Singer.

  ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/PostController.PNG)
  
Il y a une autre action qui prend un Singer comme paramètre et réagit à un POST. Encore une fois, la liaison de données, prendre les données de la requête et les désérialiser dans un Singer, est automatique. Nous pouvons également effectuer une validation à l'aide de ModelState, renvoyant un code d'état HTTP 400, BadRequest, lorsque le Singer ne passe pas la validation.

Mais parce que nous avons décoré le Controller avec l’attribut ApiController, ce code n'est plus nécessaire car la validation se fait automatiquement par l'API Web.
Ainsi, la seule chose que nous avons à faire est d'ajouter le Singer au repository et de renvoyer un code de statut 201 Créé.
Avec le résultat de l'action CreatedAtAction, nous pouvons inclure l'URL du Singer nouvellement créé dans la réponse.

Bon, maintenant que nous avons une API entièrement fonctionnelle, il est temps de la consommer à partir du Vibe.UI.

#### Consommer une API Web
Dans le Program.cs de l'application Vibe.UI (Razor page), un HttpClient est ajouté au conteneur d'injection de dépendance. 
HttpClient est la classe à utiliser pour faire des requêtes HTTP. Il est configuré pour envoyer les requêtes à l'API.
A ce stade j’introduis dans Vibe.UI une classe SingerApiService qui se chargera de faire les réelles demandes.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/Service.PNG)
 
Ce service obtient le HttpClient injecté. 

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/ServiceGetAll.PNG)
 
Dans GetAll, on l'utilise pour faire une requête GET à l'URL du Singer.
La ligne suivante garantit un code d'état HTTP dans la plage 200, ce qui indique le succès. Si un autre code d'état est renvoyé, une exception est levée ici. Après ça, les données contenues dans la réponse sont lues et désérialisées dans un IEnumerable de Singer, qui est renvoyé par la méthode.

 ![This is an image](https://github.com/issammenjli/Vibe/blob/master/Vibe.UI/wwwroot/images/AddSinger.PNG)
 
La méthode Add obtient un Singer en tant que paramètre qui est encapsulé dans un objet jsonContent. Cet objet peut ensuite être utilisé pour faire une requête POST à l'URL du Singer.
Souvent, des éléments tels que l'ID du Singer sont ajoutés par l'API. Il est important que l'appelant obtienne cette information. C'est pourquoi la réponse contiendra le Singer tel qu'il a été ajouté à la Repository, que nous pouvons désérialiser et renvoyer.

It’s done ^^

#### Conclusion :
Cette article est une bonne opportunité pour se familiariser avec les grandes lignes du ASP.net Core 6 et le Web API. Maintenant je vous invite à voir en détail chaque point pour explorer la richesse du Framework. N’hésitez pas à me faire part de vos commentaires où de vos questions😉.
 
