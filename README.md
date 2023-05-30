# Krios API and SaaS Product User Instructions

## Introduction:
Intranet owners and Corporate Communications teams can foster a culture of inclusivity by creating personalized experiences for employees. Krios is a customizable Viva Connections dashboard element that integrates with any Human Resources (HR) systems to provide employees with their personal HR data directly inside Microsoft Teams and Viva Connections.

Encouraging meaningful connections in the workforce has never been more challenging or important in the new world of hybrid work. Krios addresses this concern by enhancing your efforts to align your organization with your vision, mission, and strategic priorities, creating an additional avenue for employees to discover relevant communications and communities.

By directly integrating your HRIS and HCM platforms with Viva Connections, Krios eliminates the barrier to providing basic employee data without the need for extensive customization.

## Key Features and Information:
Krios offers the following features and information to employees:

- PTO Balance: Employees can view their remaining Paid Time Off (PTO) balance directly within the Viva Connections dashboard.
- Work Anniversary Information: Krios displays employees' work anniversary details, allowing them to celebrate milestones and foster a sense of recognition and appreciation.
- Payroll Dates: Employees can access information about upcoming payroll dates, helping them stay informed and plan accordingly.

Krios is available for your employees anywhere Microsoft Teams is accessible and supports customizations for both frontline and international employees, ensuring a tailored experience for every user.

## Installation Instructions:
Follow the steps below to install and configure Krios:

1. **Install Application through Azure Portal:**

- Access the Azure Portal ([https://portal.azure.com](https://portal.azure.com)).
- Go to "Create a Resource"
- Search for "Krios"
- Select the Krios application from the Azure Marketplace
- Select the appropriate plan that matches your company's size and needs.
- "Subscribe"!

2. **Create an Application in your Azure AD to upload data to Krios with:**

- Navigate to the Azure Active Directory (AD) portal ([https://portal.azure.com](https://portal.azure.com)).
- Create a new application in your Azure AD by following the guidelines provided in the Krios documentation.
- Take note of the application's client ID or any other required credentials for future reference.

3. **Grant the application the Krios.DataLoad permission under Krios API in Azure AD:**

- In the Azure AD portal, navigate to the Krios API.
- Locate the "Krios.DataLoad" permission and grant it to the application created in the previous step.
- Save the changes and ensure the application has the necessary permissions to interact with the Krios API.

4. **Send your data to the Krios Data Load API:**

- Refer to the samples and API documentation available at [https://github.com/kizantech/krios-samples](https://github.com/kizantech/krios-samples) for detailed instructions on how to format and send data to the Krios Data Load API.
- Use the client ID and credentials obtained earlier to authenticate your requests and upload the data successfully.

5. **Upload the SPFX Package to the App Catalog and Publish to your SharePoint portal:**

- Access your SharePoint portal and navigate to the App Catalog.
- Follow the instructions provided in the Krios documentation to upload the SPFX Package to the App Catalog.
- Publish the package to make it available for installation by users.

6. **Add the Krios Card to your Viva Connections Dashboard:**

- Open your Viva Connections Dashboard.
- Locate the option to add a new card or widget.
- Select the Krios Card from the available options and follow the prompts to configure it according to your preferences.

## Support:
If you need further assistance or encounter any issues, please reach out to our support team through the
