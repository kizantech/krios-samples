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

Krios is available for your employees anywhere Microsoft Teams or SharePoint Online is accessible and supports customizations for both frontline and international employees, ensuring a tailored experience for every user.

## Installation Instructions:
Follow the steps below to install and configure Krios:

1. **Install Application through Azure Portal:**

- Access the Microsoft [Entra Portal](https://entra.microsoft.com). Ensure that your user account has an email address associated with it in Entra so that Krios can register your organization properly. 
- Go to "Create a Resource"
- Search the Azure Marketplace for "Krios"
- Select the appropriate plan that matches your company's size and needs.
- "Subscribe"!

1. **Create an Application in your Entra ID to upload data to Krios with:**

- When you installed Krios via the Azure Marketplace, an Enterprise Application Registration was added to your Entra ID. A Global Administrator for your organization will need to:
  - Browse 
- Navigate to the [Entra Portal](https://entra.microsoft.com) again
- Create a new application in your Azure AD by following the guidelines provided in the Krios documentation.
- Take note of the application's client ID or any other required credentials for future reference.

3. **Send your data to the Krios Data Load API:**

- Refer to the [samples and API documentation available in this repository](https://github.com/kizantech/krios-samples/blob/main/samples/powershell/UploadDataToKrios.ps1) for detailed instructions on how to format and send data to the Krios Data Load API.

5. **Upload the SPFX Package to the App Catalog and Publish to your SharePoint portal:**

- Upload the Krios Viva Connections Card to your organization's App Catalog via [these instructions](https://github.com/kizantech/krios-samples/blob/main/publishing-krios-to-viva-connections.md)

6. **Add the Krios Card to your Viva Connections Dashboard:**

- Open your Viva Connections Dashboard.
- Locate the option to add a new card or widget.
- Select the Krios Card from the available options and follow the prompts to configure it according to your preferences.

## Support:
If you need further assistance or encounter any issues, please reach out to our support team through the
