# Secret Sips — Game Design

Secret Sips is a Jackbox-style party game adaptation of Dropout's
[Dirty Laundry](https://dropout.fandom.com/wiki/Dirty_Laundry): a social
deduction game where players' anonymous secrets are revealed one at a time and
everyone tries to guess whose secret it is. On the show, the culprit confesses
by taking a drink — hence the name.

Played in person: one shared context (a couch, a table), each player using
their phone as a controller.

## Setup

1. A host creates a game, choosing:
   - **Rounds** — how many rounds the game runs
   - **Min secrets** — the minimum number of secrets each player must submit
   - **Timer length** — the guessing-window time limit
2. The game gets a 6-character join code; players join with a username.
3. Before play starts, every player privately submits at least *min secrets*
   secrets. Submitting more is fine — extras simply never come up.

Because each round consumes one secret **per player**, min secrets must cover
the number of rounds. Surplus secrets give the pool some unpredictability:
you can't be sure a given secret of yours will ever appear.

## Round flow

Each round, one secret from every player is consumed, in shuffled order. For
each secret:

1. **Reveal** — the secret is shown to everyone. No host reads it aloud
   (unlike the show); the app presents it.
2. **Discussion / accusation** — freeform, in person. The owner's job is to
   deflect; everyone else's job is to interrogate.
3. **Guessing window** — players lock in on their phones who they think the
   secret belongs to. The **owner submits a decoy guess** like everyone else
   (as on the show), so vote counts don't give them away. The timer exists
   only to force guesses to actually happen — there are no producers keeping
   the take moving.
4. **Confession** — once guesses are locked, the app waits for the owner to
   tap **"It was me"**, which reveals them to all other players. The sip (and,
   in the spirit of the show, telling the full story) happens in the room.

## Scoring

- Guessing the owner correctly: **+1 point**.
- The owner fooling **every** other player (zero correct guesses): **players − 1
  points** (one per fooled player — in a 5-player game, a perfect bluff is
  worth 4). All-or-nothing: a single correct guesser voids the bluff bonus.

Highest score after the final round wins.

## Open questions

Things not yet decided — to settle before/while building the game loop:

- **Tie-break** — shared win is probably fine for a drinking game, but decide.
- **Mid-game joins/drops** — someone arrives late or taps out; what happens
  to their secrets and the per-round consumption rule?

## Follow-ups (not v1)

- **Shared main screen** — a Jackbox-style TV/laptop view showing the current
  secret, timer, and scoreboard while phones stay private controllers. v1 is
  phones-only.
- **Cocktails** — Dirty Laundry pairs each episode with a cocktail
  ("Hey Grant, What are We Drinking?"). A per-game cocktail suggestion would
  be a fun nod and explains the logo.

## Implementation status (2026-07-02)

The code predates most of this document; the game loop itself was never built
(the original controller ends at `// Draw the rest of the fucking owl`).
The task backlog lives on the
[Secret Sips Trello board](https://trello.com/b/n0fLLgii/secret-sips)
(dormant since May 2025): server-side shared state, join-over-WebSocket,
real persistence for the in-memory games list, subscriber pattern, secret
storage; client-side create/join/secret-input/round pages; investigations
into React Native and NoSQL options.

- **Server** (this repo): rewritten May 2025 from a raw-WebSocket controller
  to HotChocolate GraphQL. Only `createGame` exists; storage is an in-memory
  DAO; the subscription is a test ping. The models (`Game`, `Player`,
  `Secret{Text, IsUsed}`) match the design above.
- **Client** (`secret-sips-client`): never migrated off the old raw-WebSocket
  endpoints (`/SecretSips/Create`, `/SecretSips/Join`) — it cannot currently
  talk to this server. Join/create forms and a secret-entry form exist;
  hardcoded test values remain.
- Deployed as Portainer stacks 25 (server) / 28 (client) behind the
  GamingWings-gated tile, effectively as a landing page.

The intended transport for live game state is GraphQL subscriptions over
WebSocket (the same HotChocolate + `graphql-ws` pattern proven in DaysSince).
