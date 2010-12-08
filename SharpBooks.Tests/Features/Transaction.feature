Feature: Transaction
    Transactions should be consistent

Scenario: Transaction is invalid when it has no splits
    Given a currency 'C'
      And an empty transaction 'T' with the security 'C'
    Then transaction 'T' is invalid

Scenario: Transaction is valid when it has a single zero split
    Given a currency 'C'
	  And an account 'A' with security 'C'
      And a transaction 'T' with the following splits
        | Account | Amount |
        | A       | 0      |
    Then transaction 'T' is valid

Scenario: Transaction is invalid when it has a single nonzero split
    Given a currency 'C'
	  And an account 'A' with security 'C'
      And a transaction 'T' with the following splits
        | Account | Amount |
        | A       | 1      |
    Then transaction 'T' is invalid

Scenario: Transaction is invalid when it has unbalanced splits
    Given a currency 'C'
	  And an account 'A' with security 'C'
      And a transaction 'T' with the following splits
        | Account | Amount |
        | A       | 1      |
        | A       | -2     |
    Then transaction 'T' is invalid

Scenario: Transaction is invalid when its base currency does not match any splits
    Given a currency 'C1'
	  And an account 'A' with security 'C1'
      And a currency 'C2'
      And an empty transaction 'T' with the security 'C2'
     When the following splits are added to transaction 'T'
        | Account | Amount |
        | A       | 0      |
    Then transaction 'T' is invalid
