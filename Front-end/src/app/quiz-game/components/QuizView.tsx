import { Button, Flex } from "antd"
import { QuizContainer } from "./QuizContainer";


export function QuizView({sentenceOne, sentenceTwo, answerSentence, loadNextSentences} :
    {
        sentenceOne: string,
        sentenceTwo: string,
        answerSentence: (answer: string) => void,
        loadNextSentences: () => void,
    }
){
    return (
        <Flex vertical align="center">
                <Flex gap="small" wrap>
                    <Flex vertical gap="small" align="center">
                        <QuizContainer>
                            {sentenceOne}
                        </QuizContainer>
                        <Button color="green" shape="round" variant="solid" style={{width: '200px'}} onClick={() =>{answerSentence(sentenceOne)}}>
                            Left one?
                        </Button>
                    </Flex>
                    <Flex vertical gap="small" align="center">
                        <QuizContainer>
                            {sentenceTwo}
                        </QuizContainer>
                        <Button color="green" shape="round" variant="solid" style={{width: '200px'}} onClick={() =>{answerSentence(sentenceTwo)}}>
                            Right one?
                        </Button>
                    </Flex>
                </Flex>
                <Button shape="round" variant="solid" style={{width: '200px'}} onClick={loadNextSentences}>
                    Skip
                </Button>
            </Flex>
    )
}