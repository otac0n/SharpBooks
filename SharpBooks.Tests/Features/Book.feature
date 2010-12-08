Feature: Book
    Books hold collections of interrelated transactions, securities and accounts.

Scenario: Transactions are locked when added to a book
    Given a book
      And a currency 'C'
      And an account 'A' with security 'C'
      And a transaction 'T' with the following splits
        | Account | Amount |
        | A       | 100    |
        | A       | -100   |
     When I add the security 'C' to the book
      And I add the account 'A' to the book
      And I add transaction 'T' to the book
     Then transaction 'T' should be locked

Scenario: Transactions are unlocked when removed from a book
    Given a book
      And a currency 'C'
      And an account 'A' with security 'C'
      And a transaction 'T' with the following splits
        | Account | Amount |
        | A       | 100    |
        | A       | -100   |
     When I add the security 'C' to the book
      And I add the account 'A' to the book
      And I add transaction 'T' to the book
      And I remove transaction 'T' from the book
     Then transaction 'T' should be unlocked
