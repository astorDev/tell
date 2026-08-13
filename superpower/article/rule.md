**Description 1**: I have cars. I try to drive each of them one by one until they **stop**. If they **break down** along the way I take a new one.

- `}}`: I drive for exactly two steps. If either of them is not `{` I **break down**.
- `Placeholder`: I drive only by road consisting of `{`, `Identifier`, `}`. No more no less, exactly in that sequence, otherwise I **break down**. After `}` I **stop**
- `Text` : I drive until I see a `}`. If that the first thing I see I **break down**.

**Description 2**: I have a row of "dishes" and consumers. I show them the dishes, they have to eat them one by one. If a consumer haven't eaten anything, I move to the new one.

- `}}`: I eat only if I'm proposed two `}` immediately. I stop right after I consumed them.
- `Placeholder` : I eat only specific sequence of dishes: `{`, `Identifier`, `}`.
- `Text` : I eat util I see a `}`.