"use client";

import { App, Layout } from "antd";
import { Typography } from "antd";
import SiteHeader from "@/components/SiteHeader";

const { Title, Text } = Typography;
const { Content } = Layout

function Home() {
  return (
    <Layout style={{ minHeight: "100vh", background: "#f5f5f5" }}>
      <SiteHeader selectedKey="/" />
      <Content style={{ padding: "40px 80px" }}>
        <Title level={1}>Welcome to GetEmployed!</Title>
        <Title level={5}>Vision Statement</Title>
        <Text>
            We aim to elevate emerging junior developers and recent grads to help them find work that works for them.
            In a time where doubt and uncertainty are common when it comes to finding jobs, we offer support to young talent trying to break into the technology industry. Our service helps junior developers find positions by providing multiple resources to create one source of help in many important aspects of the industry.
            Whether it&apos;s improving professional skills like cover letter writing, matching openings to developer qualities or building a community of support, we prioritize the relevant areas that increase success.
            By providing the tools to make job finding easier and applications more likely to be accepted, we make finding a job of user interest more likely, and help employers find better candidates.
            To us, success would look like wide use of our services with quantifiable improvements in application acceptances, community and employer engagement.
        </Text>

        <Title level={5}>Vision Statement</Title>
        <Text>
            Our product aims to provide computer science job seekers with an extensive platform that makes the job hunt easy, and even fun. The CS GetEmployed website lets users search for jobs, manage and organize their applications, get notified about new relevant job postings, showcase their prior work experience to potential employers, and leave comments on job postings or reviews for companies. CS GetEmployed also provides various tools for users to improve their writing skills in order to enhance their resumes and cover letters.

            Users of CS GetEmployed are able to search for job postings that match filters that they outline, including things like the type of position, the type of employment, any specific required experience, and job location. In addition to searching for and filtering available job postings, users are able to save postings that they are interested in so that they can complete an application at a later time. Users are also able to sort their saved applications into different user-created folders. The job search is made a little more fun by providing users with a gamified interface that lets them swipe left or right to skip or save job postings they are interested in. Busier users that have less time to search for jobs can subscribe to be notified of job postings that match their experience, allowing them to save and apply for relevant jobs without needing to manually search.

            CS GetEmployed is not only geared towards job seekers, but to employers as well. Our application allows the creation of profiles that allows candidates to input their past work experience which employers will be able to view, and both employers and other users have the ability to create job postings on CS GetEmployed, either for their own company or for job postings they have seen from other sources. On CS GetEmployed, job seekers are able to see which employers have viewed their profile to get a feeling for who has an interest in their skillset. In addition to user-employer interaction, we also facilitate user-user interaction by allowing our users to leave comments on job postings detailing their own experiences with the company in the posting, providing the information to users that want to learn more about a company&apos;s culture.

            Finally, CS GetEmployed has writing tools like an action sentence designer, generic word detection, and a sentence power quiz that helps users better their skill in writing resumes and cover letters.
        </Text>

        <Title level={5}>Core Features</Title>
        <Text strong={true}>Application management</Text>
        <br/>
        <Text>
          Allows users to save posts, mark which ones they applied for, add notes, and sort them using their own personal folder structure.
        </Text>
        <br/>
        <br/>
        
        <Text strong={true}>Resume and Cover Letter Improvement</Text>
        <br/> 
        <Text>
          Lets users improve their writing skills by showing them how to structure sentences, suggest different words, or showing which words to avoid.
        </Text>
        <br/>
        <br/>
        
        <Text strong={true}>Search jobs</Text>
        <br/> 
        <Text>
          A more traditional job board that is tailored to technology positions. Filters allow users to narrow down available jobs to their desired needs and skills.
        </Text>
        <br/>
        <br/>
        
        <Text strong={true}>Dating job game</Text>
        <br/> 
        <Text>
          Going through job postings can be a boring and tedious task. We improve this by allowing users to participate in a tinder-like environment where choosing jobs is a more enjoyable task rather than a chore.
        </Text>
        <br/>
        <br/>
        
        <Text strong={true}>Add Job</Text>
        <br/> 
        <Text>
          We allow users or companies to add job postings to our website to develop an active and engaging community.
        </Text>
        <br/>
        <br/>

        <Text strong={true}>Subscribe to new postings</Text>
        <br/> 
        <Text>
          Users can configure what types of jobs they want to be notified for, so that they can be the first to apply when they come in.
        </Text>
        <br/>
        <br/>

        <Text strong={true}>Job Search Community</Text>
        <br/> 
        <Text>
          Users and companies both have their own public facing profiles to share who they are and what they&apos;re looking for with the community. We also allow users to comment on job postings, each other&apos;s profiles and company profiles to provide opinions or recommendations on postings and interviews.
        </Text>
        <br/>
        <br/>

        <Title level={5}>Technologies</Title>
        <Text><Text strong={true}>Front-end:</Text> React, JS, npm, Ant Design</Text>
        <br/>
        <Text><Text strong={true}>Back-end:</Text> C#, ASP.net, .NET</Text>
        <br/>
        <Text><Text strong={true}>Database:</Text> MySQL, Aiven</Text>
        <br/>
        <Text><Text strong={true}>Other:</Text> Github actions, Docker, Cypress</Text>

      </Content>
    </Layout>
  );
}

export default function HomePage() {
  return(
    <App>
      <Home/>
    </App>
  )
}
