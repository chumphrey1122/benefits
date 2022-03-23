import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Benefits Application</h1>
        <p>This demo application was created by <a href="mailto:christina.humphrey2@gmail.com">Christina Humphrey</a> as a simple example to share with the folks at Paylocity. Please let her know if you have any questions! The specific tech stack used in this application is:</p>
        <ul>
                <li>ASP.NET Core (.NET 6.0) and C# for the server side code</li>
                <li>Azure SQL database</li>
                <li>React for client-side code</li>
                <li>Bootstrp for layout and styling</li>
                <li>Nunit for unit testing (server-side)</li>
                <li>Azure App Service as host environment</li>
        </ul>
        <p>Due to time constraints, this application is incomplete. There are multiple ways it can be enhanced and extended, some of which include:</p>
        <ul>
            <li><strong>Authentication</strong>. This involves creating an Identity Server to return a JWT token to the JavaScript that can be sent with all AJAX requests to our backend, where it will be validated with middleware. This may also require:
                <ul>
                    <li>User account management and permissions infrastructure</li>
                    <li>Password recovery (via email)</li>
                </ul>
                </li>
            <li><strong>Paging, sorting and filtering employees</strong>. The ability to filter and sort the list of employees, which would also be paged in order to support larger numbers of individuals.</li>
            <li><strong>Enhanced business rules</strong>. The current business rules are very simple (single company, fixed pay for all employees, single benefits plan), and could be enhanced in a more full-service application.</li>
            <li><strong>Reports</strong>. The ability to provide historical reports for what an employee actually paid in each pay period. This would support possible changes in costs or dependents with time.</li>
            <li><strong>"Self-serve" options</strong>. An employee would be able to enter their own dependents (but not see any other employee's details).</li>
            </ul>
        <p>To get started, just click on the <a href="/employees">Employees</a> navigation link in the top right, and see what happens next!</p>
      </div>
    );
  }
}
