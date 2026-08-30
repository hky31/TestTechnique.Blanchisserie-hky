# Récapitulatif du projet Blanchisserie - ajout Henri Ky

## 1. Environnement technique

| Composant       | Version                                                  |
| --------------- | -------------------------------------------------------- |
| Backend         | ASP.NET Core / .NET 9.0                                  |
| Frontend        | Angular 20.3 (standalone components, **zoneless**)       |
| Node.js         | 22 LTS (via Homebrew)                                    |
| Base de données | SQLite (remplace SQL Server LocalDB, incompatible macOS) |
| ORM             | Entity Framework Core                                    |
| Auth            | JWT (JSON Web Token)                                     |

---

## 2. Architecture backend

### Modèles (Entities)

**`Order.cs`** - les commandes faites par les utilisateurs, Valider/Refuser par les admins [structure reprise de User/Role/UserRole]

```csharp
public class Order
{
    public int Id { get; set; }
    public ICollection<OrderOrderItem> OrderList { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; } // Waiting / Validated / Refused -- enum
    public string Commentaire { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}
```

- Pas de champs `FirstName/LastName/Email` dupliqués : ces informations sont déja présentes dans `User` (choix : éviter la duplication).
- Relation `Order → User` : on ne peut pas supprimer un `User` ayant des commandes, on garde tout l'historique des commandes passées.

**`OrderItem.cs`** — catalogue d'articles disponibles pour une commande (Id, ItemName, Price). Pas de lien direct avec Order, les articles sont indépendants.

**`OrderOrderItem.cs`** — table d'association Order ↔ OrderItem (équivalent de `UserRole`), pour retrouver les articles d'une commande, permet à un même article d'être utilisé dans plusieurs commandes.

### DTOs

| DTO                | Usage                                                                                                                                                          |
| ------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `OrderRequestDto`  | Création/mise à jour : `OrderItemIds: List<int>`, `Commentaire`, `Status` (valeur Waiting par défaut à la création, seul un admin peut mettre à jour ce champ) |
| `OrderResponseDto` | Réponse API : retourne l'ensemble des données de `Order` + `User` (nom/prénom/email) + liste d'articles                                                        |
| `OrderItemDto`     | Représentation d'un article (catalogue ou ligne de commande)                                                                                                   |

### Services

- `OrderService` : CRUD des commandes
- `OrderItemService` : CRUD du catalogue d'articles

### Contrôleurs et sécurité par rôle

| Route                              | Méthode | Accès                 | Usage                                        |
| ---------------------------------- | ------- | --------------------- | -------------------------------------------- |
| `POST /api/order/create`           | POST    | Authentifié           | Créer une commande (UserId récupéré du token |
| `GET /api/order/get/user/{userId}` | GET     | Authentifié           | Historique personnel                         |
| `GET /api/order/get`               | GET     | **Admin uniquement**  | Tableau de toutes les commandes              |
| `GET /api/order/get/{id}`          | GET     | Propriétaire ou Admin | Détail d'une commande                        |
| `PUT /api/order/update/{id}`       | PUT     | **Admin uniquement**  | Valider/Refuser une commande                 |
| `GET /api/orderitems`              | GET     | Authentifié           | Catalogue d'articles disponibles             |

---

## 3. Architecture frontend

### Modèles (`models/order.models.ts`)

- `OrderStatus` (enum), `OrderItem`, `OrderRequest`, `OrderResponse`

### Services

- `OrderService` — CRUD des commandes
- `OrderItemService` — CRUD du catalogue d'articles

### Composants

| Composant                         | Rôle                                                                                           | Route / Intégration                    |
| --------------------------------- | ---------------------------------------------------------------------------------------------- | -------------------------------------- |
| `OrderComponent`                  | Formulaire de commande (sélection catalogue + ajout/retrait)                                   | intégré dans Dashboard                 |
| `OrderHistoryComponent`           | Historique des commandes de l'utilisateur connecté, se recharge automatiquement après création | intégré dans Dashboard                 |
| `OrderTableComponent`             | Tableau de toutes les commandes, actions Valider/Refuser                                       | intégré dans Admin                     |
| `OrderConfirmationModalComponent` | Pop-up récapitulatif réutilisable (avec ou sans actions selon `showActions`)                   | utilisé par OrderTable et OrderHistory |

---

## 4. Fonctionnalités développées

- ✅ **Fonctionnalité 1 — Commande sécurisée** : formulaire authentifié, sélection d'articles depuis un catalogue en base, commentaire facultatif
- ✅ **Fonctionnalité 2 — Validation admin** : tableau de toutes les commandes, pop-up récapitulatif, actions Valider/Refuser (uniquement sur commandes "En attente")
- ✅ **Fonctionnalité 3 — Suivi** : historique personnel de commandes, statut visible, pop-up récapitulatif en lecture seule, rafraîchissement automatique après création

---

## 5. Points restants / améliorations possibles

- 🟡 Styling SCSS à finaliser sur l'ensemble des composants
- 🟡 Gestion d'un éventuel CRUD admin pour le catalogue d'articles (actuellement géré uniquement via seed)
- 🟡 Tests unitaires/d'intégration non couverts dans ce développement
