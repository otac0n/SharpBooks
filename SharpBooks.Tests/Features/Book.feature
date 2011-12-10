Feature: Book
    Books hold collections of interrelated transactions, securities and accounts.

Scenario: Transactions are locked when added to a book
    Given a book
      And a currency 'C'
      And an account 'A' with security 'C'
      And a transaction 'T' with the following splits
        | Account | Security | Amount |
        | A       | C        | 100    |
        | A       | C        | -100   |
     When I add the security 'C' to the book
      And I add the account 'A' to the book
      And I add transaction 'T' to the book
     Then transaction 'T' should be locked

Scenario: Transactions are unlocked when removed from a book
    Given a book
      And a currency 'C'
      And an account 'A' with security 'C'
      And a transaction 'T' with the following splits
        | Account | Security | Amount |
        | A       | C        | 100    |
        | A       | C        | -100   |
     When I add the security 'C' to the book
      And I add the account 'A' to the book
      And I add transaction 'T' to the book
      And I remove transaction 'T' from the book
     Then transaction 'T' should be unlocked

Scenario: Accounts without securites may be added to books
    Given a book
      And an account 'A' with no security
     Then I should be able to add the account 'A' to the book

Scenario: Accounts without securites may have any securities in its splits
    Given a book
      And a currency 'C'
      And a currency 'Q'
      And an account 'A' with no security
      And a transaction 'T' with base security 'C' and the following splits
        | Account | Security | Amount |
        | A       | C        | 100    |
        | A       | Q        | -100   |
     When I add the security 'C' to the book
      And I add the security 'Q' to the book
      And I add the account 'A' to the book
     Then I should be able to add transaction 'T' to the book

Scenario: Accounts without securites may not have transactions where the securities have not been added to the book
    Given a book
      And a currency 'C'
      And a currency 'Q'
      And an account 'A' with no security
      And a transaction 'T' with base security 'C' and the following splits
        | Account | Security | Amount |
        | A       | C        | 100    |
        | A       | Q        | -100   |
     When I add the security 'C' to the book
      And I add the account 'A' to the book
     Then I should not be able to add transaction 'T' to the book
